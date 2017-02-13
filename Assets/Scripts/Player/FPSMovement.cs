using UnityEngine;
using System.Collections;
using MassiveNet;

public class FPSMovement : MonoBehaviour {
	
    public float speed = 5f;
    public float jumpForce = 5f;
    public float sprintSpeed = 8f;

    public Animator anim;
    private CharacterController cc;

    private Vector3 moveTo = Vector3.zero;
    private float velY = 0f;
    private bool crouching = false;

    void Awake(){
        cc = transform.GetChild(0).GetComponent<CharacterController>();
    }
    void Update(){
        moveTo = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
        moveTo = Camera.main.transform.TransformDirection(moveTo);
        moveTo = moveTo.normalized*speed;

        Sprinting();
        Crouching();
        Jumping();

        velY += Physics.gravity.y * Time.deltaTime;
        moveTo.y = velY;
        cc.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x,0f,Camera.main.transform.forward.z));

        anim.SetFloat("speed", Mathf.Abs(moveTo.normalized.x)+Mathf.Abs(moveTo.normalized.z));
        cc.Move(moveTo*Time.deltaTime);
    }

    private void Sprinting(){
        if ( !crouching ){
            if ( Input.GetKey(KeyCode.LeftShift) ){
                if ( !anim.GetBool("sprint") ){
                    anim.SetBool("sprint",true);
                }
                moveTo = moveTo.normalized*sprintSpeed;
            }
            if ( Input.GetKeyUp(KeyCode.LeftShift) ){
                anim.SetBool("sprint",false);
            }
        }
    }
    private void Crouching(){
        if ( Input.GetKey(KeyCode.LeftControl) ){
            if ( !anim.GetBool("crouch") ){
                anim.SetBool("crouch",true);
                crouching = true;

                cc.center = new Vector3(0f,0.75f,0f);
                cc.height = 1.5f;
            }
            moveTo /= 2f;
        }
        if ( Input.GetKeyUp(KeyCode.LeftControl) ){
            anim.SetBool("crouch",false);
            crouching = false;

            cc.center = new Vector3(0f,1f,0f);
            cc.height = 2f;
        }
    }
    private void Jumping(){
        if ( cc.isGrounded ){
            velY = -1;
            if ( Input.GetButtonDown("Jump") ){
                velY = jumpForce;
                anim.SetBool("jump",true);
            } else {
                anim.SetBool("jump",false);
            }
        }
    }
}
