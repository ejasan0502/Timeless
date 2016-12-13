using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class ChatClient : MonoBehaviour {

    private NetSocket socket;

    void Awake(){
        socket = GetComponent<NetSocket>();
    }
    void Start(){
        socket.RegisterRpcListener(this);
    }

    [NetRPC]
    private void ReceiveChatMessage(string charName, string msg, NetConnection conn){
        UI ui = UIManager.instance.GetUI("ChatUI");
        if ( ui != null ){
            ((ChatUI)ui).AddText(charName + ": " + msg);
        }
    }
}
