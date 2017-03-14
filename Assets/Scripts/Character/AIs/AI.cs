using UnityEngine;
using System.Collections;

// Handles generic AI logic
[RequireComponent(typeof(CharacterMovement))]
public class AI : Character {

    private AIState aiState = AIState.idle;

    protected CharacterMovement charMovt;
    protected AIRadius aiRadius;
    protected Animator anim;

    private Vector3 moveToPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private bool move = false;
    protected bool attack = false;

    protected Character target = null;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
        aiRadius = GetComponentInChildren<AIRadius>();
        anim = GetComponent<Animator>();

        moveToPosition = transform.position;
    }
    void Update(){
        Movement();
    }
    void FixedUpdate(){
        StateMachine();
    }

    // Handle AI states logic
    private void StateMachine(){
        switch (aiState){
        case AIState.idle:
            Idle();
            break;
        case AIState.combat:
            Combat();
            break;
        case AIState.cast:
            Cast();
            break;
        }
    }
    // Handles AI movement
    private void Movement(){
        if ( move ){
            velocity = moveToPosition - transform.position;
            velocity.Normalize();

            charMovt.Move(velocity);

            Quaternion rot = Quaternion.LookRotation(velocity);
            rot.x = 0f;
            rot.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f*Time.deltaTime);

            if ( Vector3.Distance(transform.position, moveToPosition) < 1f ){
                Stop();
            }
        }

        Animate();
    }
    // Handles animations
    private void Animate(){
        Vector3 direction = transform.TransformDirection(velocity);
        anim.SetFloat(Settings.instance.anim_velocity_z, Mathf.Abs(direction.z));
        anim.SetInteger(Settings.instance.anim_attack, attack ? Random.Range(1,3) : 0);
    }

    // Logic when AI has no target
    protected virtual void Idle(){}
    // Logic when AI has a target
    protected virtual void Combat(){}
    // Logic when AI is casting
    protected virtual void Cast(){}
    
    // Move AI to position
    protected void MoveTo(Vector3 position){
        moveToPosition = position;
        move = true;
    }
    // Have AI stop moving
    protected void Stop(){
        move = false;
        velocity = Vector3.zero;
        charMovt.Move(velocity);
    }

    // Set current AI state to
    protected void SetState(AIState state){
        aiState = state;
    }
    // Set desired target
    public void SetTarget(Character c){
        target = c;
    }
}
