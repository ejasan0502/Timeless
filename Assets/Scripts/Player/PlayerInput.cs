using UnityEngine;
using System.Net;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    public float speed = 500f;
    private CharacterController cc;

    private Vector3 moveTo = Vector3.zero;
    public Animator anim;

    void Awake(){
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>() ?? transform.GetComponentInChildren<Animator>();
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
        if ( anim != null ) anim.SetFloat("speed", cc.velocity.magnitude);
    }
    private void Move(Vector3 moveTo){
        Instantiate(Resources.Load("Effects/MovePointer"), moveTo, Quaternion.identity);

        this.moveTo = moveTo;
        transform.LookAt(new Vector3(moveTo.x,transform.position.y,moveTo.z));
    }

    public void SetAnim(Animator anim){
        this.anim = anim;
    }
}
