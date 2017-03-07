using UnityEngine;
using System.Collections;

// Handles input from player other than main camera
public class UserInput : MonoBehaviour {

    private CharacterMovement charMovt;
    private WeaponHandler weaponHandler;
    private Animator anim;

    private KeyCode lastKeyPressed;
    private float dodgeTime = 0f;
    private bool dodging = false;

    private bool crouching = false;
    private bool proning = false;
    private bool sprinting = false;

    private bool hideMouse = true;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
        weaponHandler = GetComponent<WeaponHandler>();
        anim = GetComponent<Animator>();
    }
    void Start(){
        SetCursorView();
    }
    void Update(){
        Reload();
        Attack();
        Jumping();
        Sprinting();
        Crouching();
        Prone();

        Movement();

        HideViewCursor();
    }

    // Handles movement logic
    private void Movement(){
        if ( !dodging ){
            Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            charMovt.Move(v);
        }

        charMovt.Animate(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
    }
    // Handle jumping logic
    private void Jumping(){
        if ( Input.GetButtonUp("Jump") ){
            if ( proning ){
                proning = false;
                charMovt.Prone(proning);
            } else if ( crouching ){
                crouching = false;
                charMovt.Crouch(crouching);
            } else {
                charMovt.Jump();
            }
        }
    }
    // Handle sprinting logic
    private void Sprinting(){
        if ( crouching || proning ) return;

        if ( charMovt.isGrounded ){
            if ( Input.GetButton("Sprint") ){
                sprinting = true;
                charMovt.Sprint(sprinting);
            }
            if ( Input.GetButtonUp("Sprint") ){
                sprinting = false;
                charMovt.Sprint(sprinting);
            }
        }
    }
    // Handle dodging logic
    private void Dodging(){
        if ( crouching || proning ) return;

        KeyCode currentPressed = GetKeyDown(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        if ( currentPressed != KeyCode.None ){
            if ( lastKeyPressed != KeyCode.None ){
                if ( GetKeyDown(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D) == lastKeyPressed ){
                    charMovt.Dodge(currentPressed);
                    dodging = true;
                } else {
                    lastKeyPressed = KeyCode.None;
                }
            } else {
                lastKeyPressed = currentPressed;
            }
            dodgeTime = Time.time;
        } else if ( Time.time - dodgeTime >= 1f ){
            lastKeyPressed = KeyCode.None;
            dodging = false;
        }
    }
    // Handle crouching logic
    private void Crouching(){
        if ( Input.GetButton("Crouch") ){
            if ( !crouching ) {
                crouching = true;
                charMovt.Crouch(crouching);
            }
        }
        if ( Input.GetButtonUp("Crouch") ){
            crouching = false;
            charMovt.Crouch(crouching);
        }
    }
    // Handle proning logic
    private void Prone(){
        if ( Input.GetButtonUp("Prone") ){
            proning = !proning;
            charMovt.Prone(proning);
        }
    }
    // Handle attacking logic
    private void Attack(){
        if ( dodging || sprinting || weaponHandler.currentWeapon == null ) return;

        // Primary Fire
        if ( Input.GetButtonDown("Fire1") ){
            weaponHandler.currentWeapon.SinglePrimaryFire();
        }
        if ( Input.GetButton("Fire1") ){
            weaponHandler.currentWeapon.PrimaryFire();
        }
        if ( Input.GetButtonUp("Fire1") ){
            anim.SetInteger(Settings.instance.anim_attack, 0);
        }

        // Secondary Fire
        if ( Input.GetButtonDown("Fire2") ){
            weaponHandler.currentWeapon.SecondaryFire();
        }
        if ( Input.GetButtonUp("Fire2") ){
            weaponHandler.currentWeapon.SecondaryFireEnd();
        }
    }
    // Handle reloading logic
    private void Reload(){
        if ( weaponHandler.currentWeapon == null ) return;

        if ( Input.GetButtonDown("Reload") ){
            weaponHandler.currentWeapon.AltFire();
        }
    }

    // Hide/View cursor input
    private void HideViewCursor(){
        if ( Input.GetKeyDown(KeyCode.Escape) ){
            hideMouse = !hideMouse;

            SetCursorView();
        }
    }
    // Hide.View cursor
    private void SetCursorView(){
        Cursor.visible = !hideMouse;
        Cursor.lockState = hideMouse ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // Detect if either these keys are pressed
    private KeyCode GetKeyDown(params KeyCode[] keys){
        foreach (KeyCode key in keys){
            if ( Input.GetKeyDown(key) ){
                return key;
            }
        }
        
        return KeyCode.None;
    }
}
