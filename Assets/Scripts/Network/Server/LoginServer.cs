using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

[RequireComponent(typeof(NetSocket))] 
public class LoginServer : MonoBehaviour {

    private NetSocket socket;
    private Dictionary<IPEndPoint, Account> sessions = new Dictionary<IPEndPoint,Account>();
    
    private List<Account> accounts = new List<Account>(){
        new Account("ejasan0502@gmail.com", "ejasan0502", "temp123")
    };

    void Awake(){
        socket = GetComponent<NetSocket>();

        socket.RegisterRpcListener(this);
        socket.Events.OnClientDisconnected += EndSession;
    }

    [NetRPC]
    private void LoginRequest(string username, string password, NetConnection conn) {
        // Validate credentials
        foreach (Account acc in accounts){
            if ( acc.username == username || acc.email == username ){
                if ( acc.password == password ){
                    CreateSession(acc, conn);
                    socket.Send("OnLoginResponse", conn, true, "");
                } else {
                    socket.Send("OnLoginResponse", conn, false, "Invalid password");
                }
                return;
            }
        }
        socket.Send("OnLoginResponse", conn, false, "Do not recognize username/email");
    }

    private void CreateSession(Account acc, NetConnection conn){
        sessions.Add(conn.Endpoint, acc);
    }
    private void EndSession(NetConnection conn) {
        // Check if session exists
        if ( sessions.ContainsKey(conn.Endpoint) ){
            sessions.Remove(conn.Endpoint);
        }
    }
}
