using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(AudioSource))]
public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;
    public float freeFallTime = 1.25f;
	public float jumpForce = 250f;
    public float jetPackForce = 10f;
    public float jetPackConsumeRate = 75f;
    public float sprintConsumeRate = 25f;
    public AudioClip footsteps;
    public AudioClip jetpack;

    public bool IsGrounded {
        get {
            return isGrounded;
        }
    }

    private Animator anim;
    private AudioSource audioSource;
    private Character character;
    private Rigidbody rb;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    private bool jumping = false;
    public bool sprinting = false;
    private bool freefalling = false;
    private bool dodging = false;
    private bool crouching = false;
    private bool proning = false;

    private bool canJetPack = false;
    private bool jetPacking = false;
    public bool isGrounded = false;
    
	void Awake() {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody>();

        SetupAnimator();
	}
    void Update(){
        if ( sprinting ){
            if ( character.currentCharStats.stamina > 0 ){
                character.currentCharStats.stamina -= sprintConsumeRate*Time.deltaTime;
            } else {
                sprinting = false;
            }
        }

        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothVelocity, 0.15f);

        CheckGround();
        JetpackRecov();
    }
    void FixedUpdate(){
        rb.MovePosition(rb.position + velocity*Time.fixedDeltaTime);
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

        if ( audioSource && !audioSource.isPlaying ){
            audioSource.clip = footsteps;
            audioSource.Play();
        }

	    targetVelocity = v.normalized * speed * velocityMultipler;
    }
    // Have character jump
    public void Jump(){
        if ( IsGrounded && !jumping ){
            rb.AddForce(transform.up*jumpForce);
            jumping = true;
            StartCoroutine(FreeFall());
        } else if ( canJetPack ){
            jetPacking = true;
        }
    }
    // Have character use jet pack
    public void OnJetPackHold(){
        if ( jetPacking ){
            if ( character ) {
                if ( character.currentCharStats.jetpack > 0 ){
                    character.currentCharStats.jetpack -= jetPackConsumeRate*Time.deltaTime;
                    rb.AddForce(transform.up*jetPackForce);
                } else {
                    jetPacking = false;
                }
            }
        }
    }
    // Have character stop using jet pack
    public void OnJetPackEnd(){
        jetPacking = false;
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

        if ( character.currentCharStats.stamina < 1 ){
            sprinting = false;
        }
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
        if ( IsGrounded && (forward != 0 || strafe != 0) ){
            if ( audioSource.clip != footsteps ){
                audioSource.clip = footsteps;
            }
            if ( !audioSource.isPlaying ){
                audioSource.Play();
            }

            // Increase pitch if sprinting
            audioSource.pitch = sprinting ? 2f : crouching ? 0.5f : 1f;
        } else if ( jetPacking ){
            if ( audioSource.clip != jetpack ){
                audioSource.clip = jetpack;
            }
            if ( !audioSource.isPlaying ){
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        anim.SetFloat(Settings.instance.anim_velocity_x, strafe);
        anim.SetFloat(Settings.instance.anim_velocity_z, sprinting ? forward*2f : forward);

        anim.SetBool(Settings.instance.anim_grounded, IsGrounded);
        anim.SetBool(Settings.instance.anim_jump, jumping);
        anim.SetBool(Settings.instance.anim_free_fall, freefalling);
        anim.SetBool(Settings.instance.anim_free_fall, jetPacking);
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
        RaycastHit hit;
        Debug.DrawRay(transform.position,-transform.up*1.1f,Color.blue);
        if ( Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 1.1f, 1 << LayerMask.NameToLayer("Environment")) ){
            if ( hit.collider.gameObject.isStatic ){
                isGrounded = true;
            } else {
                isGrounded = false;
            }
        } else {
            isGrounded = false;
        }

        if ( IsGrounded ){
            dodging = false;
            jumping = false;
            freefalling = false;
            canJetPack = true;
            StopCoroutine(FreeFall());
        }
    }
    // Apply jetpack recovery
    private void JetpackRecov(){
        if ( jetPacking ) return;

        // Increase jet pack fuel over time
        if ( character ){
            if ( character.currentCharStats.jetpack < character.maxCharStats.jetpack ){
                character.currentCharStats.jetpack += character.currentCharStats.jetpackRecov*Time.deltaTime;
                if ( character.currentCharStats.jetpack > character.maxCharStats.jetpack ){
                    character.currentCharStats.jetpack = character.maxCharStats.jetpack;
                }
            }
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

