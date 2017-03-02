using UnityEngine;
using System.Collections;

// Handles input from player other than main camera
public class UserInput : MonoBehaviour {

    private CharacterMovement charMovt;
    public KeyCode lastKeyPressed;
    private float dodgeTime = 0f;
    private bool dodging = false;
    private bool crouching = false;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
    }
    void Update(){
        Dodging();
        Jumping();
        Sprinting();
        Crouching();
        Movement();
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
            charMovt.Jump();
        }
    }
    // Handle sprinting logic
    private void Sprinting(){
        if ( charMovt.isGrounded ){
            if ( Input.GetButton("Sprint") ){
                charMovt.Sprint(true);
            }
            if ( Input.GetButtonUp("Sprint") ){
                charMovt.Sprint(false);
            }
        }
    }
    // Handle dodging logic
    private void Dodging(){
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
