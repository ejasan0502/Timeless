using UnityEngine;
using System.Collections;

// Handles generic AI logic
[RequireComponent(typeof(CharacterMovement))]
public class AI : Character {

    private AIState aiState = AIState.idle;

    protected CharacterMovement charMovt;
    protected AIRadius aiRadius;

    private Vector3 moveToPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool move = false;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
        aiRadius = GetComponentInChildren<AIRadius>();

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
        case AIState.chase:
            Chase();
            break;
        case AIState.attack:
            Attack();
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
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f*Time.deltaTime);

            if ( Vector3.Distance(transform.position, moveToPosition) < 1f ){
                move = false;
                Debug.Log("Stop Movement");
                velocity = Vector3.zero;
                charMovt.Move(velocity);
            }
        }

        Vector3 direction = transform.TransformDirection(velocity);
        charMovt.Animate(Mathf.Abs(direction.z), direction.x);
    }

    // Logic when AI has no target
    protected virtual void Idle(){}
    // Logic when AI has a target but is outside attack range
    protected virtual void Chase(){}
    // Logic when AI has a target and within attack range
    protected virtual void Attack(){}
    // Logic when AI is casting
    protected virtual void Cast(){}
    
    // Move AI to position
    protected void MoveTo(Vector3 position){
        moveToPosition = position;
        move = true;

        Debug.Log("Moving to " + position);
    }
}
