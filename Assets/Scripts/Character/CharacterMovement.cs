using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;
    public float freeFallTime = 1.25f;
	public float jumpHeight = 2.0f;
    public AudioClip footsteps;

    public bool IsGrounded {
        get {
            return cc.isGrounded;
        }
    }

    private Animator anim;
    private CharacterController cc;
    private AudioSource audioSource;

    private float velY = 0f;
    private Vector3 velocity = Vector3.zero;

    private bool jumping = false;
    private bool sprinting = false;
    private bool freefalling = false;
    private bool dodging = false;
    private bool crouching = false;
    private bool proning = false;
    
	void Awake() {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        SetupAnimator();
	}
	void Update() {
	    // We apply gravity manually for more tuning control
	    velY += Physics.gravity.y * Time.deltaTime;
        velocity.y = velY;

        cc.Move(velocity*Time.deltaTime);
 
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

	    velocity = v * velocityMultipler;
	    velocity = transform.TransformDirection(velocity);
	    velocity *= speed;
    }
    // Have character jump
    public void Jump(){
        if ( cc.isGrounded && !jumping ){
            velY = jumpHeight;
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
        if ( cc.isGrounded && (forward != 0 || strafe != 0) ){
            if ( !audioSource.isPlaying )
                audioSource.Play();

            // Increase pitch if sprinting
            audioSource.pitch = sprinting ? 2f : crouching ? 0.5f : 1f;
        } else {
            audioSource.Stop();
        }

        anim.SetFloat(Settings.instance.anim_velocity_x, strafe);
        anim.SetFloat(Settings.instance.anim_velocity_z, sprinting ? forward*2f : forward);

        anim.SetBool(Settings.instance.anim_grounded, cc.isGrounded);
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
    // Check if character is grounded
    private void CheckGround(){
        if ( cc.isGrounded ){
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

