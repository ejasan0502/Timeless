using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;
    public float freeFallTime = 1.25f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
    
    public bool isGrounded { get; private set; }

    private Animator anim;
    private Rigidbody rb;

    private Vector3 targetVelocity = Vector3.zero;
    private bool jumping = false;
    private bool sprinting = false;
    private bool freefalling = false;
    
	void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

	    rb.freezeRotation = true;
	    rb.useGravity = false;

        isGrounded = false;
	}
	void Update() {
	    if ( isGrounded ) {
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rb.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rb.AddForce(velocityChange, ForceMode.VelocityChange);
	    }
 
	    // We apply gravity manually for more tuning control
	    rb.AddForce(new Vector3 (0, Physics.gravity.y * rb.mass, 0));
 
	    CheckGround();
	}

    public void Move(Vector3 v){
	    targetVelocity = v * (sprinting ? 2f : 1f);
	    targetVelocity = transform.TransformDirection(targetVelocity);
	    targetVelocity *= speed;
    }
    public void Jump(){
        if ( !jumping ){
            rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
            jumping = true;
            StartCoroutine(FreeFall());
        }
    }
    public void Sprint(bool b){
        sprinting = b;
    }
    public void Animate(float forward, float strafe){
        anim.SetFloat(Settings.instance.anim_velocity_x, strafe);
        anim.SetFloat(Settings.instance.anim_velocity_z, sprinting ? forward*2f : forward);

        anim.SetBool(Settings.instance.anim_grounded, isGrounded);
        anim.SetBool(Settings.instance.anim_jump, jumping);
        anim.SetBool(Settings.instance.anim_free_fall, freefalling);
    }

    private IEnumerator FreeFall(){
        yield return new WaitForSeconds(freeFallTime);
        freefalling = true;
    }
	private float CalculateJumpVerticalSpeed() {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * -Physics.gravity.y);
	}
    private void CheckGround(){
        RaycastHit hit;
        if ( Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f) ){
            if ( hit.collider.gameObject.isStatic ){
                isGrounded = true;
            }
        }

        if ( isGrounded ){
            jumping = false;
            freefalling = false;
            StopCoroutine(FreeFall());
        }
    }
}

