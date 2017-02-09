using UnityEngine;
using System.Collections;
using MassiveNet;

public class ActionControl : MonoBehaviour {
	
    private NetView view;

    void Awake(){
        view = GetComponent<NetView>();
    }
    void Update(){
        if ( Input.GetMouseButtonDown(0) ){
            view.SendReliable("AttackInput", RpcTarget.Server);
        }
    }

}
