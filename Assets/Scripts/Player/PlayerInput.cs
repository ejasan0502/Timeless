using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    public float speed = 500f;
    private CharacterController cc;

    private Character character;

    void Awake(){
        cc = GetComponent<CharacterController>();
        character = GetComponent<Character>();
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
                        character.SetState(CharacterState.idle);
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
    }
    private void Select(GameObject o){
        Character c = o.GetComponent<Character>();
        if ( c != null ){
            UI targetInfo = UIManager.instance.GetUI("TargetInfoUI");
            if ( targetInfo != null ){
                character.SetTarget(c);
                character.SetState(CharacterState.combat);

                targetInfo.SetDisplay(true);
                TargetInfoUI targetInfoUI = (TargetInfoUI) targetInfo;
                targetInfoUI.SetTarget(c);
            }
        }
    }
}
