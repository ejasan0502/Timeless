using UnityEngine;
using System.Net;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    public float speed = 500f;
    private CharacterController cc;

    private Vector3 moveTo = Vector3.zero;

    void Awake(){
        cc = GetComponent<CharacterController>();
        moveTo = transform.position;
    }
    void Update(){
        Movement();
    }

    private void Movement(){
        if ( Input.GetMouseButtonDown(0) ){
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Terrain")) ){
                moveTo = hit.point;
            }
        }

        cc.SimpleMove( (moveTo-transform.position).normalized*speed*Time.deltaTime );
    }
}
