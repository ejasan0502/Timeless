using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class ChatServer : MonoBehaviour {

    private NetSocket socket;

    void Awake(){
        socket = GetComponent<NetSocket>();
    }
    void Start(){
        socket.RegisterRpcListener(this);
    }

    [NetRPC]
    private void SendChatMessage(string charName, string msg, NetConnection conn){
        foreach (NetConnection con in socket.Connections){
            if ( con == socket.Self ) continue;
            socket.Send("ReceiveChatMessage", con, charName, msg);
        }
    }
}
