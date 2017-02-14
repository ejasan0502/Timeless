using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Character : MonoBehaviour {

    public string id;
    public bool undying;            // Cannot die
    public bool immortal;           // Cannot be damaged
    public float atkRange = 2f;
    public float atkRate = 1f;
    public CharStats maxStats;
    public CharStats currentStats;
    public EquipStats maxEquipStats;
    public EquipStats currentEquipStats;

    public bool canMove { get; private set; }
    public bool canCast { get; private set; }

    private Character target = null;
    private List<Character> targets = null;
    private CharacterController cc;
    private Animator anim;
    private CharacterState state = CharacterState.idle;
    private CharacterModel charModel;

    private Vector3 moveTo = Vector3.zero;
    private float startAtkTime = 0f;
    private float noTargetTime = 0f;

    private NetView view;

    private int atkCounter = 0;
    private WeaponType primaryWeapType;

    public bool IsAlive {
        get {
            return currentStats.hp > 0;
        }
    }
    public bool HasTarget {
        get {
            return target != null;
        }
    }
    public Character Target {
        get {
            return target;
        }
    }
    public NetView View {
        get {
            return view ?? GetComponent<NetView>();
        }
    }
    public static Character main {
        get {
            return GameObject.FindObjectOfType<PlayerOwner>().GetComponent<Character>();
        }
    }

    void Awake(){
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        view = GetComponent<NetView>();

        maxStats = new CharStats(1f);
        currentStats = new CharStats(maxStats);
        currentStats.movtSpd = 250f;

        maxEquipStats = new EquipStats(1f);
        currentEquipStats = new EquipStats(maxEquipStats);

        canMove = true;
        canCast = true;

        primaryWeapType = WeaponType.unarmed;
    }
    void Update(){
        StateMachine();
        Movement();
    }

    public void SetTarget(Character c){
        target = c;
        SetState("combat");
    }
    public void SetAnim(Animator a){
        anim = a;
    }
    public void SetCharModel(CharacterModel charModel){
        this.charModel = charModel;
    }

    public void SetAnimState(string paramName, bool val){
        if ( anim != null )
            anim.SetBool(paramName, val);
    }
    public void SetAnimState(string paramName, float val){
        if ( anim != null )
            anim.SetFloat(paramName, val);
    }
    public void SetAnimState(string paramName, int val){
        if ( anim != null )
            anim.SetInteger(paramName, val);
    }

    public void Chase(){
        if ( target != null ){
            SetState("chase");
            view.SendReliable("StateInput", RpcTarget.Server, "chase");
        } else
            Debug.Log("No target");
    }
    public void SetInputRestrict(bool canMove, bool canCast){
        this.canMove = canMove;
        this.canCast = canCast;
    }
    public void SetCanMove(bool b){
        canMove = b;
    }
    public void SetCanCast(bool b){
        canCast = b;
    }
    public void SetPrimary(WeaponType wt){
        primaryWeapType = wt;
    }

    [NetRPC]
    public void Fire(){
        if ( anim == null ) return;

        if ( primaryWeapType == WeaponType.unarmed ){
            if ( !anim.GetBool("combat") ) anim.SetBool("combat",true);

            atkCounter++;
            if ( atkCounter > Global.MAX_ATK_COUNTER ){
                atkCounter = 1;
            }
            anim.SetInteger("attack",atkCounter);
        } else if ( primaryWeapType == WeaponType.guns ){
            Instantiate(Resources.Load("Effects/muzzleFlash"),charModel.nodes[(int)EquipType.primary].GetChild(0).GetChild(0),false);
        }
    }
    [NetRPC]
    public void Move(Vector3 moveTo){
        this.moveTo = moveTo;
        transform.LookAt(new Vector3(moveTo.x,transform.position.y,moveTo.z));
    }
    [NetRPC]
    public void SetState(string stateName){
        state = (CharacterState)Enum.Parse(typeof(CharacterState),stateName);

        if ( state == CharacterState.combat ){
            SetAnimState("combat", true);
            if ( target == null ){
                noTargetTime = Time.time;
            }
        } else if ( state == CharacterState.idle ){
            SetAnimState("combat", false);
        }
    }
    [NetRPC]
    public void SetTarget(string id){
        Debug.Log("Targetting " + id);
        Character character = null;
        foreach (Character c in GameObject.FindObjectsOfType<Character>()){
            if ( c != null ){
                if ( c.id == id ){
                    character = c;
                    break;
                }
            }
        }
        SetTarget(character);
    }
    [NetRPC]
    public void Hit(float rawDmg){
        if ( !immortal ){
            currentStats.hp -= rawDmg;

            if ( Server.instance == null ){
                GameObject o = (GameObject) Instantiate(Resources.Load("FloatingText"));
                o.transform.SetParent(transform);
                o.transform.localPosition = new Vector3(0f,1f,0f);
                Text text = o.transform.GetChild(0).GetComponent<Text>();
                text.text = rawDmg+"";
                text.color = Color.red;
            }

            CheckDeath();
        } else {
            Debug.Log("Object is immortal.");
        }
    }
    [NetRPC]
    public void BasicAttack(){
        if ( target != null ){
            float rawDmg = UnityEngine.Random.Range(currentEquipStats.minPhysDmg, currentEquipStats.maxPhysDmg);
            target.Hit(rawDmg);
        }
    }
    [NetRPC]
    public void SetTargets(string[] ids){
        targets = new List<Character>();
        foreach (string s in ids){
            Character c = GameObject.FindObjectsOfType<Character>().Where<Character>(ch => ch.id == s).FirstOrDefault();
            if ( c != null ){
                targets.Add(c);
            }
        }
    }

    private void CheckDeath(){
        if ( !IsAlive ){
            if ( undying ){
                currentStats.hp = maxStats.hp;
            } else {
                gameObject.AddComponent<DestroyDelay>();
            }
        }
    }

    private void StateMachine(){
        switch(state){
        case CharacterState.combat:
        if ( target == null && Time.time - noTargetTime > 3f ){
            SetState("idle");
        }
        break;
        case CharacterState.chase:
        if ( target != null ){
            Move(target.transform.position);
            if ( Vector3.Distance(transform.position,target.transform.position) < atkRange ){
                if ( Time.time - startAtkTime > atkRate ){
                    startAtkTime = Time.time;
                    SetAnimState("attack", true);
                } else {
                    SetAnimState("attack", false);
                }
            }
        } else {
            SetState("combat");
        }
        break;
        case CharacterState.attacking:
        if ( target != null ){
            if ( Time.time - startAtkTime > atkRate ){
                startAtkTime = Time.time;
                SetAnimState("attack", true);
            }
        } else {
            SetState("combat");
        }
        break;
        }
    }
    private void Movement(){
        if ( moveTo != Vector3.zero ){
            cc.SimpleMove( (moveTo-transform.position).normalized*currentStats.movtSpd*Time.deltaTime );
            if ( anim != null )
                SetAnimState("speed", Vector3.Distance(transform.position,moveTo) > 1.15f ? 1f : 0f);
        }
    }

}
