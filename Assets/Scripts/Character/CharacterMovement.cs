using UnityEngine;
using System.Collections;

// Handles movement logic and animation
public class CharacterMovement : MonoBehaviour {

    private Animator anim;
    private Vector3 velocity = Vector3.zero;

    void Awake(){
        // Grab animator from model
        anim = GetComponentInChildren<Animator>();
    }
    void Update(){
        Animate();
    }

    // Animate character
    private void Animate(){
        anim.SetFloat(Settings.instance.anim_velocity_x, velocity.x);
        anim.SetFloat(Settings.instance.anim_velocity_z, velocity.z);
    }

}
