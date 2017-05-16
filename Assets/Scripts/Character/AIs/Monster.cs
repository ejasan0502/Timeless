using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles monster type AIs
public class Monster : AI {

    [Header("-AI Info-")]
    public Vector3 wanderStartPos;
    public float wanderDelay = 10f;
    public float wanderDistance = 10f;
    public float viewField = 0.5f;      // -1 to 1 where 1 is front and -1 is behind

    [Header("-Audio-")]
    public List<AudioClip> atkSounds;

    [Header("-Debugging-")]
    public bool disableWandering = false;
    public bool disableTargetting = false;
    public bool showRays = false;

    private bool canWander = true;
    private bool canAttack = true;

    void Start(){
        wanderStartPos = transform.position;
    }

    // Have AI wander around when without a target
    protected override void Idle(){
        Wander();
    }
    // Have AI move towards target until target is within attack range
    protected override void Combat(){
        if ( target != null && target.IsAlive ){
            if ( showRays ) Debug.DrawLine(transform.position, target.transform.position, Color.red);
            if ( Vector3.Distance(transform.position, target.transform.position) < currentCombatStats.atkRange ){
                Attack();
            } else {
                Chase();
            }
        } else {
            SetState(AIState.idle);
        }
    }

    // Move randomly around the area
    private void Wander(){
        if ( disableWandering ) return;

        if ( target == null ){
            if ( canWander ){
                Vector3 moveTo = Random.insideUnitCircle*wanderDistance;
                moveTo.z = moveTo.y;
                moveTo.y = 0f;
                moveTo = wanderStartPos + moveTo;

                RaycastHit hit;
                Vector3 startRayPos = new Vector3(moveTo.x, 1000f, moveTo.z);
                if ( Physics.Raycast(startRayPos, Vector3.down, out hit, 2000f, 1 << LayerMask.NameToLayer("Environment")) ){
                    if ( showRays ) Debug.DrawLine(transform.position, hit.point, Color.blue, 1f);
                    if ( hit.collider.gameObject.isStatic ){
                        MoveTo(hit.point);

                        canWander = false;
                        StartCoroutine(WanderDelay());
                    }
                }
            }

            LookForTarget();
        } else {
            SetState(AIState.combat);
        }
    }
    // Delay for selecting new point of interest
    private IEnumerator WanderDelay(){
        yield return new WaitForSeconds(wanderDelay);
        canWander = true;
    }
    // Have AI attack the target
    private void Attack(){
        Stop();
        if ( canAttack ){
            atkCounter = Random.Range(1,atkSounds.Count);

            attack = true;
            canAttack = false;
            StartCoroutine(AttackDelay());
        }
    }
    // Delay for attacking the target
    private IEnumerator AttackDelay(){
        yield return new WaitForSeconds(currentCombatStats.atkRate);
        canAttack = true;
    }
    // Have AI chase the target
    private void Chase(){
        MoveTo(target.transform.position);
    }
}
