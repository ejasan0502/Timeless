using UnityEngine;
using System.Collections;

// Handles movement logic and animation
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;

    private Animator anim;
    private CharacterController cc;

    private Vector3 velocity = Vector3.zero;

    void Awake(){
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void Update(){
        velocity = transform.TransformDirection(velocity);
        velocity = velocity.normalized*speed;

        velocity += Physics.gravity*Time.deltaTime;

        cc.Move(velocity*Time.deltaTime);
    }
    
    // Animate character
    public void Animate(float forward, float strafe){
        anim.SetFloat(Settings.instance.anim_velocity_x, strafe);
        anim.SetFloat(Settings.instance.anim_velocity_z, forward);
    }
    public void Move(Vector3 velocity){
        this.velocity = velocity;
    }
}
