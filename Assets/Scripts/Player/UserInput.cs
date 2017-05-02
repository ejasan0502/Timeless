using UnityEngine;
using System.Collections;

// Handles input from player other than main camera
public class UserInput : MonoBehaviour {

    private CameraControl camControl;
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
        camControl = GetComponentInChildren<CameraControl>();
        charMovt = GetComponent<CharacterMovement>();
        weaponHandler = GetComponent<WeaponHandler>();
        anim = GetComponent<Animator>();
    }
    void Start(){
        SetCursorView();
    }
    void Update(){
        if ( !GameManager.instance.ignoreControlsInput ){
            Reload();
            Attack();
            Jumping();
            Sprinting();
            Crouching();
            Prone();

            Movement();

            Hotkeys();
        }

        HideViewCursor();
    }

    // Handles movement logic
    private void Movement(){
        if ( !dodging ){
            Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if ( !charMovt.IsUnderWater ) v = transform.TransformDirection(v);
            else v = Camera.main.transform.TransformDirection(v);

            charMovt.Move(v);
        }

        charMovt.Animate(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
    }
    // Handle jumping/jetpack logic
    private void Jumping(){
        if ( Input.GetButtonDown("Jump") ){
            this.Log("OnJumpDown");
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
        if ( Input.GetButton("Jump") ){
            charMovt.OnJetPackHold();
        }
        if ( Input.GetButtonUp("Jump") ){
            this.Log("OnJumpUp");
            charMovt.OnJetPackEnd();
        }
    }
    // Handle sprinting logic
    private void Sprinting(){
        if ( crouching || proning ) return;

        if ( charMovt.IsGrounded ){
            if ( Input.GetButtonDown("Sprint") ){
                this.Log("OnSprintDown");
                sprinting = true;
                charMovt.Sprint(sprinting);
            }
        }
        if ( Input.GetButtonUp("Sprint") ){
            this.Log("OnSprintUp");
            sprinting = false;
            charMovt.Sprint(sprinting);
        }
    }
    // Handle dodging logic
    private void Dodging(){
        if ( crouching || proning ) return;

        KeyCode currentPressed = GetKeyDown(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        if ( currentPressed != KeyCode.None ){
            if ( lastKeyPressed != KeyCode.None ){
                if ( GetKeyDown(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D) == lastKeyPressed ){
                    this.Log("OnDodgeDown");
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
                camControl.Crouch(crouching);
            }
        }
        if ( Input.GetButtonUp("Crouch") ){
            this.Log("OnCrouchUp");
            crouching = false;
            charMovt.Crouch(crouching);
            camControl.Crouch(crouching);
        }
    }
    // Handle proning logic
    private void Prone(){
        if ( Input.GetButtonUp("Prone") ){
            this.Log("OnProneUp");
            proning = !proning;
            charMovt.Prone(proning);
        }
    }
    // Handle attacking logic
    private void Attack(){
        if ( dodging || sprinting || weaponHandler && weaponHandler.currentWeapon == null ) return;

        // Primary Fire
        if ( Input.GetButtonDown("Fire1") ){
            this.Log("OnPrimaryFireDown");
            weaponHandler.currentWeapon.SinglePrimaryFire();
        }
        if ( Input.GetButton("Fire1") ){
            weaponHandler.currentWeapon.PrimaryFire();
        }
        if ( Input.GetButtonUp("Fire1") ){
            this.Log("OnPrimaryFireUp");
            anim.SetInteger(Settings.instance.anim_attack, 0);
        }

        // Secondary Fire
        if ( Input.GetButtonDown("Fire2") ){
            this.Log("OnSecondaryFireDown");
            weaponHandler.currentWeapon.SecondaryFire();
        }
        if ( Input.GetButtonUp("Fire2") ){
            this.Log("OnSecondaryFireUp");
            weaponHandler.currentWeapon.SecondaryFireEnd();
        }
    }
    // Handle reloading logic
    private void Reload(){
        if ( weaponHandler && weaponHandler.currentWeapon == null ) return;

        if ( Input.GetButtonDown("Reload") ){
            weaponHandler.currentWeapon.AltFire();
        }
    }
    // Handle weapon switching
    private void Hotkeys(){
        if ( !weaponHandler ) return;

        KeyCode keyPressed = GetKeyDown(KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
                                        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0);
        if ( keyPressed != KeyCode.None ){
            switch (keyPressed){
            case KeyCode.Alpha1:
            weaponHandler.Equip(1);
            break;
            case KeyCode.Alpha2:
            weaponHandler.Equip(2);
            break;
            case KeyCode.Alpha3:
            weaponHandler.Equip(3);
            break;
            case KeyCode.Alpha4:
            weaponHandler.Equip(4);
            break;
            case KeyCode.Alpha5:
            weaponHandler.Equip(5);
            break;
            case KeyCode.Alpha6:
            weaponHandler.Equip(6);
            break;
            case KeyCode.Alpha7:
            weaponHandler.Equip(7);
            break;
            case KeyCode.Alpha8:
            weaponHandler.Equip(8);
            break;
            case KeyCode.Alpha9:
            weaponHandler.Equip(9);
            break;
            case KeyCode.Alpha0:
            weaponHandler.Equip(0);
            break;
            }
        }
    }

    // Hide/View cursor input
    private void HideViewCursor(){
        if ( Input.GetKeyDown(KeyCode.Escape) ){
            this.Log("HideViewCursor");
            hideMouse = !hideMouse;

            SetCursorView();
        }
    }
    // Hide.View cursor
    private void SetCursorView(){
        this.Log("SetCursorView");
        GameManager.instance.SetControlsInput(!hideMouse);
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

    // View/Hide mouse cursor
    public void HideMouse(bool hide){
        this.Log("HideMouse");
        hideMouse = hide;
        SetCursorView();
    }
}
