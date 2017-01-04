using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Character : MonoBehaviour {

    public string id;
    public bool undying;            // Cannot die
    public bool immortal;           // Cannot be damaged
    public float atkRange = 1f;
    public float atkRate = 1f;
    public CharStats maxStats;
    public CharStats currentStats;

    private Character target = null;
    private CharacterController cc;
    private Animator anim;
    private CharacterState state = CharacterState.idle;

    private Vector3 moveTo = Vector3.zero;
    private float startAtkTime = 0f;

    private NetView view;

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
        currentStats.movtSpd = 500f;
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
    public void Hit(float rawDmg){
        if ( !immortal ){
            currentStats.hp -= rawDmg;
            CheckDeath();
        } else {
            Debug.Log("Object is immortal.");
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

        if ( anim != null ){
            if ( state == CharacterState.combat ){
                SetAnimState("combat", true);
            } else if ( state == CharacterState.idle ){
                SetAnimState("combat", false);
            }
        }
    }
    [NetRPC]
    public void SetTarget(string id){
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
        if ( target != null ){
            if ( Vector3.Distance(transform.position,target.transform.position) > atkRange ){
                SetState("chase");
            }
        }
        break;
        case CharacterState.chase:
        if ( target != null ){
            Move(target.transform.position);
            if ( Vector3.Distance(transform.position,target.transform.position) < atkRange ){
                SetState("attacking");
            }
        } else {
            SetState("idle");
        }
        break;
        case CharacterState.attacking:
        if ( target != null ){
            if ( Time.time - startAtkTime > atkRate ){
                startAtkTime = Time.time;
                SetAnimState("attack", true);
            }
        } else {
            SetState("idle");
        }
        break;
        }
    }
    private void SetAnimState(string paramName, bool val){
        if ( anim != null )
            anim.SetBool(paramName, val);
    }
    private void SetAnimState(string paramName, float val){
        if ( anim != null )
            anim.SetFloat(paramName, val);
    }
    private void Movement(){
        if ( moveTo != Vector3.zero ){
            cc.SimpleMove( (moveTo-transform.position).normalized*currentStats.movtSpd*Time.deltaTime );
            SetAnimState("speed", cc.velocity.magnitude);
        }
    }

}
