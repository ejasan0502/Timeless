using UnityEngine;
using System.Collections;

// Handles input from player other than main camera
public class UserInput : MonoBehaviour {

    private CharacterMovement charMovt;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
    }
    void Update(){
        Jumping();
        Sprinting();
        Movement();
    }

    // Handles movement logic
    private void Movement(){
        Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        charMovt.Move(v);
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
}
