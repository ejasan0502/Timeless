using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Character : MonoBehaviour {

    public bool undying;            // Cannot die
    public bool immortal;           // Cannot be damaged
    public float atkRange = 1f;
    public float atkRate = 1f;
    public CharStats maxStats;
    public CharStats currentStats;

    private Character target = null;
    private CharacterController cc;
    private Animator anim;
    public CharacterState state = CharacterState.idle;

    private Vector3 moveTo = Vector3.zero;
    private float startAtkTime = 0f;

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

    void Awake(){
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        moveTo = transform.position;
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
        SetState(CharacterState.chase);
    }
    public void SetState(CharacterState cs){
        state = cs;

        if ( anim != null ){
            if ( state == CharacterState.combat ){
                anim.SetBool("combat",true);
            } else if ( state == CharacterState.idle ){
                anim.SetBool("combat",false);
            } else if ( state == CharacterState.attacking ){
                anim.SetBool("attack",true);
            }
        }
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
                SetState(CharacterState.chase);
            }
        }
        break;
        case CharacterState.chase:
        if ( target != null ){
            Move(target.transform.position);
            if ( Vector3.Distance(transform.position,target.transform.position) < atkRange ){
                SetState(CharacterState.attacking);
            }
        } else {
            SetState(CharacterState.idle);
        }
        break;
        case CharacterState.attacking:
        if ( target != null ){
            if ( Time.time - startAtkTime > atkRate ){
                startAtkTime = Time.time;
                anim.SetBool("attack",true);
            }
        } else {
            SetState(CharacterState.idle);
        }
        break;
        }
    }
    private void Movement(){
        cc.SimpleMove( (moveTo-transform.position).normalized*currentStats.movtSpd*Time.deltaTime );
        if ( anim != null ) anim.SetFloat("speed", cc.velocity.magnitude);
    }
    private void Attack(){
        anim.SetBool("attack", true);
    }

}
