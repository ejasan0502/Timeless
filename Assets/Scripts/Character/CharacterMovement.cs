using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;
    public float freeFallTime = 1.25f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
    public AudioClip footsteps;

    public bool isGrounded { get; private set; }

    private Animator anim;
    private Rigidbody rb;
    private AudioSource audio;

    private Vector3 targetVelocity = Vector3.zero;
    private bool jumping = false;
    private bool sprinting = false;
    private bool freefalling = false;
    private bool dodging = false;
    private bool crouching = false;
    private bool proning = false;
    
	void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

	    rb.freezeRotation = true;
	    rb.useGravity = false;

        isGrounded = false;

        SetupAnimator();
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

    // Move character based on input or AI
    public void Move(Vector3 v){
        float velocityMultipler = 1f;
        if ( crouching ){
            velocityMultipler = 0.50f;
        } else if ( proning ){
            velocityMultipler = 0.25f;
        } else if ( sprinting ){
            velocityMultipler = 2f;
        }

	    targetVelocity = v * velocityMultipler;
	    targetVelocity = transform.TransformDirection(targetVelocity);
	    targetVelocity *= speed;
    }
    // Have character jump
    public void Jump(){
        if ( !jumping ){
            rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
            jumping = true;
            StartCoroutine(FreeFall());
        }
    }
    // Have character dodge in given direction based on input
    public void Dodge(KeyCode key){
        dodging = true;
        
        switch (key){
            case KeyCode.A:
            Move(Vector3.left*0.35f);
            break;
            case KeyCode.D:
            Move(Vector3.right*0.35f);
            break;
            case KeyCode.W:
            Move(Vector3.forward);
            break;
            case KeyCode.S:
            Move(Vector3.back*0.5f);
            break;
        }
    }
    // Have character sprint
    public void Sprint(bool b){
        sprinting = b;
    }
    // Have character crouch
    public void Crouch(bool b){
        crouching = b;
    }
    // Have character prone
    public void Prone(bool b){
        proning = b;
    }
    // Animate character;
    public void Animate(float forward, float strafe){
        if ( anim == null ) return;

        // Play footsteps sound
        if ( isGrounded && (forward != 0 || strafe != 0) ){
            if ( !audio.isPlaying )
                audio.Play();

            // Increase pitch if sprinting
            audio.pitch = sprinting ? 2f : 1f;
        } else {
            audio.Stop();
        }

        anim.SetFloat(Settings.instance.anim_velocity_x, strafe);
        anim.SetFloat(Settings.instance.anim_velocity_z, sprinting ? forward*2f : forward);

        anim.SetBool(Settings.instance.anim_grounded, isGrounded);
        anim.SetBool(Settings.instance.anim_jump, jumping);
        anim.SetBool(Settings.instance.anim_free_fall, freefalling);
        anim.SetBool(Settings.instance.anim_dodge, dodging);
        anim.SetBool(Settings.instance.anim_crouch, crouching);
        anim.SetBool(Settings.instance.anim_prone, proning);
    }

    // Character is freefalling after a certain duration
    private IEnumerator FreeFall(){
        yield return new WaitForSeconds(freeFallTime);
        freefalling = true;
    }
    // Smooth jump speed
	private float CalculateJumpVerticalSpeed() {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * -Physics.gravity.y);
	}
    // Check if character is grounded
    private void CheckGround(){
        RaycastHit hit;
        if ( Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f) ){
            if ( hit.collider.gameObject.isStatic ){
                isGrounded = true;
            }
        } else {
            isGrounded = false;
        }

        if ( isGrounded ){
            dodging = false;
            jumping = false;
            freefalling = false;
            StopCoroutine(FreeFall());
        }
    }
    // Grab animators from children and apply them to parent object
    private void SetupAnimator(){
        Animator[] animators = GetComponentsInChildren<Animator>();
        if ( animators.Length > 0 ){
            for (int i = animators.Length-1; i >= 0; i--){
                if ( animators[i] != anim ){
                    anim.runtimeAnimatorController = animators[i].runtimeAnimatorController;
                    anim.avatar = animators[i].avatar;
                    Destroy(animators[i]);
                }
            }
        }
    }
}

