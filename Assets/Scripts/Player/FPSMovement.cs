using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour {
	
    public float speed = 10f;
    public float jumpForce = 100f;

    private Animator anim;
    private CharacterController cc;
    private Vector3 moveTo = Vector3.zero;
    private float velY = 0f;

    void Awake(){
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void Update(){
        moveTo = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
        moveTo = Camera.main.transform.TransformDirection(moveTo);
        moveTo *= speed;

        if ( cc.isGrounded ){
            velY = -1;
            if ( Input.GetButtonDown("Jump") ){
                velY = jumpForce;
                anim.SetBool("jump",true);
            } else {
                anim.SetBool("jump",false);
            }
        }

        velY += Physics.gravity.y * Time.deltaTime;
        moveTo.y = velY;
        transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x,0f,Camera.main.transform.forward.z));

        anim.SetFloat("speed", Input.GetAxis("Vertical"));
        cc.Move(moveTo*Time.deltaTime);
    }

}
