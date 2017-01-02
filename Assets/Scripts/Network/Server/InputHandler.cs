using UnityEngine;
using System.Collections;
using MassiveNet;

public class InputHandler : MonoBehaviour {

    private NetView view;
    private Character character;

    void Awake(){
        view = GetComponent<NetView>();
        character = GetComponent<Character>();
    }

    [NetRPC]
    private void MoveInput(Vector3 moveTo){
        character.Move(moveTo);
        view.SendReliable("Move", RpcTarget.NonControllers, moveTo);
    }

}
