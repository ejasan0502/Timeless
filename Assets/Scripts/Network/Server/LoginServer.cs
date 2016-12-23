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

    private static LoginServer _instance;
    public static LoginServer instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<LoginServer>();
            }
            return _instance;
        }
    }

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
    [NetRPC]
    private void PlayerCreateRequest(string baseModel, NetConnection conn){
        if ( sessions.ContainsKey(conn.Endpoint) ){
            sessions[conn.Endpoint].baseModel = baseModel;
        } else {
            Debug.LogError("Session is unavailable.");
        }
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

    public static Account GetAccount(IPEndPoint key){
        return instance.sessions.ContainsKey(key) ? instance.sessions[key] : null;
    }
}
