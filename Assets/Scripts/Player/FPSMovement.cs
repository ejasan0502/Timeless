using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour {
	
    public float speed = 10f;

    private Animator anim;
    private CharacterController cc;
    private Vector3 moveTo = Vector3.zero;

    void Awake(){
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void Update(){
        moveTo = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
        moveTo = Camera.main.transform.TransformDirection(moveTo);
        moveTo *= speed;
        
        moveTo += Physics.gravity;
        transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x,transform.position.y,Camera.main.transform.forward.z));

        anim.SetFloat("speed", Input.GetAxis("Vertical"));
        cc.Move(moveTo*Time.deltaTime);
    }

}
