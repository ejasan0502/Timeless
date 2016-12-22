using UnityEngine;
using System.Net;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    public float speed = 500f;
    private CharacterController cc;

    private Vector3 moveTo = Vector3.zero;
    private Animator anim;

    void Awake(){
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        moveTo = transform.position;
    }
    void Start(){
        BirdEyeCameraControl becc = Camera.main.gameObject.AddComponent<BirdEyeCameraControl>();
        becc.SetFollow(transform);
    }
    void Update(){
        Movement();
    }

    private void Movement(){
        if ( Input.GetMouseButtonDown(0) ){
            if ( !UIManager.instance.InDeadZone(Input.mousePosition) ){
                RaycastHit hit;
                if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Terrain")) ){
                    Move(hit.point);
                }
            }
        }

        cc.SimpleMove( (moveTo-transform.position).normalized*speed*Time.deltaTime );
        anim.SetFloat("speed", cc.velocity.magnitude);
    }

    [NetRPC]
    private void Move(Vector3 moveTo){
        this.moveTo = moveTo;
        transform.rotation = Quaternion.LookRotation(moveTo, Vector3.up);
    }
}
