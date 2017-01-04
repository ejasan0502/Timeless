using UnityEngine;
using System;
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
    [NetRPC]
    private void StateInput(string stateName){
        character.SetState(stateName);
        view.SendReliable("SetState", RpcTarget.NonControllers, stateName);
    }
    [NetRPC]
    private void SetTargetInput(string id){
        character.SetTarget(id);
        view.SendReliable("SetTarget", RpcTarget.NonControllers, id);
    }

}
