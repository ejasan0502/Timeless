using UnityEngine;
using System.Collections;

// Handles monster type AIs
public class Monster : AI {

    [Header("-AI Info-")]
    public Vector3 wanderStartPos;
    public float wanderDelay = 10f;
    public float wanderDistance = 10f;
    public float viewField = 0.5f;      // -1 to 1 where 1 is front and -1 is behind

    [Header("-Debugging-")]
    public bool disableTargetting = false;
    public bool showRays = false;

    private Character target = null;
    private bool canWander = true;

    void Start(){
        wanderStartPos = transform.position;
    }

    // Have AI wander around when without a target
    protected override void Idle(){
        Wander();
    }

    // Move randomly around the area
    private void Wander(){
        if ( target == null ){
            if ( canWander ){
                Vector3 moveTo = Random.insideUnitCircle*wanderDistance;
                moveTo.z = moveTo.y;
                moveTo.y = 0f;
                moveTo = wanderStartPos + moveTo;

                RaycastHit hit;
                Vector3 startRayPos = new Vector3(moveTo.x, 1000f, moveTo.z);
                if ( Physics.Raycast(startRayPos, Vector3.down, out hit, 2000f, 1 << LayerMask.NameToLayer("Environment")) ){
                    if ( showRays ) Debug.DrawLine(transform.position, hit.point, Color.red, wanderDelay);
                    if ( hit.collider.gameObject.isStatic ){
                        MoveTo(hit.point);

                        canWander = false;
                        StartCoroutine(WanderDelay());
                    }
                }
            }

            LookForTarget();
        }
    }
    // Delay for selecting new point of interest
    private IEnumerator WanderDelay(){
        yield return new WaitForSeconds(wanderDelay);
        canWander = true;
    }
    // Looks for a desired target
    private void LookForTarget(){
        if ( target != null || disableTargetting ) return;
        
        Character desiredTarget = null;
        if ( aiRadius.enemiesWithinRange.Count > 0 ){
            float distance = 100f;

            foreach (Character c in aiRadius.enemiesWithinRange){
                Vector3 direction = c.transform.position - transform.position;
                float dot = Vector3.Dot(direction.normalized, transform.forward);

                // Check if in view
                if ( dot < viewField ){
                    // Look for closest
                    float d = Vector3.Distance(transform.position, c.transform.position);
                    if ( d < distance ){
                        desiredTarget = c;
                        distance = d;
                    }
                }
            }
        }
            
        SetTarget(desiredTarget);
    } 

    // Set desired target
    public void SetTarget(Character c){
        target = c;

        if ( target != null ){
            Debug.Log("Target found!");
        }
    }
}
