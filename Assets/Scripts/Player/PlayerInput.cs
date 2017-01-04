using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    private Character character;
    private NetView view;

    void Awake(){
        character = GetComponent<Character>();
        view = GetComponent<NetView>();
    }
    void Start(){
        BirdEyeCameraControl becc = Camera.main.gameObject.AddComponent<BirdEyeCameraControl>();
        becc.SetFollow(transform);
    }
    void Update(){
        if ( Input.GetMouseButtonDown(0) ){
            if ( !UIManager.instance.InDeadZone(Input.mousePosition) ){
                RaycastHit hit;
                if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f) ){
                    if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ){
                        character.SetState("idle");
                        Move(hit.point);
                    } else if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Selectable") ){
                        Select(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void Move(Vector3 moveTo){
        Instantiate(Resources.Load("Effects/MovePointer"), moveTo, Quaternion.identity);

        character.Move(moveTo);
        view.SendReliable("MoveInput", RpcTarget.Server, moveTo);
    }
    private void Select(GameObject o){
        Character c = o.GetComponent<Character>();
        if ( c != null ){
            UI targetInfo = UIManager.instance.GetUI("TargetInfoUI");
            if ( targetInfo != null ){
                character.SetTarget(c);
                //view.SendReliable("SetTargetInput", RpcTarget.Server, c.id);

                targetInfo.SetDisplay(true);
                TargetInfoUI targetInfoUI = (TargetInfoUI) targetInfo;
                targetInfoUI.SetTarget(c);
            }
        }
    }
}
