using UnityEngine;
using System.Collections;

// Handles input from player other than main camera
public class UserInput : MonoBehaviour {

    private CharacterMovement charMovt;

    void Awake(){
        charMovt = GetComponent<CharacterMovement>();
    }
    void Update(){
        Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        charMovt.Move(v);
        charMovt.Animate(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
    }
}
