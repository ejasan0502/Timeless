using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(AudioSource),typeof(Animator),typeof(Rigidbody))]
[RequireComponent(typeof(Character),typeof(WeaponHandler),typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour {

    public float freeFallTime = 1.25f;
	public float jumpForce = 250f;
    public float jetPackForce = 10f;
    public float jetPackConsumeRate = 75f;
    public float sprintConsumeRate = 25f;
    public AudioClip footsteps;
    public AudioClip jetpack;
    public AudioClip swimming;

    public bool IsGrounded {
        get {
            return isGrounded;
        }
    }
    public bool IsUnderWater {
        get {
            return underwater;
        }
    }

    private Animator anim;
    private AudioSource audioSource;
    private Character character;
    private Rigidbody rb;
    private WeaponHandler weaponHandler;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    private bool jumping = false;
    private bool sprinting = false;
    private bool freefalling = false;
    private bool dodging = false;
    private bool crouching = false;
    private bool proning = false;

    private bool canJetPack = false;
    private bool jetPacking = false;
    public bool isGrounded = false;

    public bool underwater = false;
    
	void Awake() {
        weaponHandler = GetComponent<WeaponHandler>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody>();

        SetupAnimator();
	}
    void Update(){
        // Consume stamina while sprinting
        if ( sprinting ){
            if ( character.currentCharStats.stamina > 0 ){
                character.currentCharStats.stamina -= sprintConsumeRate*Time.deltaTime;
            } else {
                sprinting = false;

                if ( weaponHandler.currentWeapon != null ){
                    weaponHandler.charModel.transform.localPosition = weaponHandler.currentWeapon.camPosOffset;
                    weaponHandler.charModel.transform.localEulerAngles = weaponHandler.currentWeapon.camRotOffset;
                }
            }
        }

        CheckWater();

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

	    targetVelocity = v.normalized * character.currentCharStats.movementSpeed * (1f - character.WeightPercent) * velocityMultipler;
    }
    // Have character jump
    public void Jump(){
        if ( underwater ){
            this.Log("Cannot jump while underwater");
            return;
        }

        if ( IsGrounded && !jumping ){
            this.Log("Jump");
            rb.AddForce(transform.up*jumpForce);
            jumping = true;
            StartCoroutine(FreeFall());
        } else if ( canJetPack ){
            this.Log("Use jetpack");
            jetPacking = true;
        }
    }
    // Have character use jet pack
    public void OnJetPackHold(){
        if ( !underwater ){
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
        if ( underwater ) {
            this.Log("Cannot sprint while underwater.");
            return;
        }

        sprinting = b;

        if ( character.currentCharStats.stamina < 1 ){
            this.Log("Not enough stamina.");
            sprinting = false;
        }

        //if ( weaponHandler != null ){
        //    if ( sprinting ){
        //        weaponHandler.charModel.transform.localPosition = Vector3.Lerp(weaponHandler.charModel.transform.localPosition,weaponHandler.charModel.originalPos, 15*Time.deltaTime);
        //        weaponHandler.charModel.transform.localEulerAngles = Vector3.Lerp(weaponHandler.charModel.transform.localEulerAngles,weaponHandler.charModel.originalRot, 15*Time.deltaTime);   
        //    } else if ( weaponHandler.currentWeapon != null ){
        //        weaponHandler.charModel.transform.localPosition = weaponHandler.currentWeapon.camPosOffset;
        //        weaponHandler.charModel.transform.localEulerAngles = weaponHandler.currentWeapon.camRotOffset;
        //    }
        //}
    }
    // Have character crouch
    public void Crouch(bool b){
        if ( underwater ){
            this.Log("Cannot crouch underwater");
            return;
        }

        this.Log(b ? "Crouch" : "Stand");
        crouching = b;
    }
    // Have character prone
    public void Prone(bool b){
        if ( underwater ) {
            this.Log("Cannot prone underwater");
            return;
        }

        this.Log(b ? "Go prone" : "Get out of prone");
        proning = b;
    }
    // Animate character;
    public void Animate(float forward, float strafe){
        if ( anim == null ) return;

        // Play footsteps sound
        if ( IsGrounded && !underwater && (forward != 0 || strafe != 0) ){
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
        } else if ( underwater && (forward != 0 || strafe != 0) ){
            if ( audioSource.clip != swimming ){
                audioSource.clip = swimming;
            }
            if ( !audioSource.isPlaying ){
                audioSource.Play();
            }
        } else if ( IsGrounded && (forward == 0 && strafe == 0) ){
            audioSource.Pause();
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
        anim.SetBool(Settings.instance.anim_swim, underwater);
    }

    // Character is freefalling after a certain duration
    private IEnumerator FreeFall(){
        yield return new WaitForSeconds(freeFallTime);
        freefalling = true;
    }
    // Check if character is grounded
    private void CheckGround(){
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up*0.2f, Color.red);
        if ( Physics.Raycast(transform.position, -transform.up, out hit, 0.2f, 1 << LayerMask.NameToLayer("Environment")) ){
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
    // Check if character is underwater
    private void CheckWater(){
        RaycastHit hit;
        if ( Physics.Raycast(transform.position+transform.up*100f, -transform.up, out hit, (1 << LayerMask.NameToLayer("Self")) | (1 << LayerMask.NameToLayer("Water")) ) ){
            if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Water") ){
                if ( !underwater ) {
                    underwater = true;
                    weaponHandler.Unequip();
                }   
            } else {
                underwater = false;
            }
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
    // Play sound
    private void PlaySound(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
}

