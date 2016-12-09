using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class LoginClient : MonoBehaviour {

    public string serverIp = "127.0.0.1";
    public int serverPort = 17000;

    public Text msgText;
    public InputField username;
    public InputField password;
    public Button loginBtn;

    private NetSocket socket;
    private NetConnection loginServer;

    void Start() {
        socket = Client.instance.Socket;

        socket.RegisterRpcListener(this);
        socket.Events.OnConnectedToServer += ConnectedToServer;
        socket.Events.OnSocketStart += ConnectToLoginServer;
        socket.Events.OnFailedToConnect += ConnectFailed;
        socket.Events.OnDisconnectedFromServer += DisconnectedFromServer;
    }
    void Update(){
        if ( Input.GetKeyDown(KeyCode.Tab) ){
            EventSystem.current.SetSelectedGameObject( EventSystem.current.currentSelectedGameObject == username.gameObject ? password.gameObject : username.gameObject );
        }
        if ( Input.GetKeyDown(KeyCode.Return) ){
            Login();
        }
    }

    private void ConnectToLoginServer() {
        socket.Connect(serverIp + ":" + serverPort);
    }
    private void ConnectFailed(IPEndPoint endpoint) {
        if ( loginServer != null ) return;
        ConnectToLoginServer();
    }
    private void DisconnectedFromServer(NetConnection connection) {
        if (loginServer == connection) loginServer = null;
    }
    private void ConnectedToServer(NetConnection connection) {
        if ( loginServer != null ) return;
        ConnectedToLoginServer(connection);
    }
    private void ConnectedToLoginServer(NetConnection connection) {
        loginServer = connection;
    }

    [NetRPC]
    private void OnLoginResponse(bool loginSuccess, string msg, NetConnection conn){
        // Check for successful login
        if ( loginSuccess ){
            socket.Send("SpawnRequest", conn);
            Destroy(gameObject);
        } else {
            msgText.text = msg;   
        }
    }

    public void Login(){
        if ( username.text == "" || password.text == "" ) return;

        socket.Send("LoginRequest", loginServer, username.text, password.text);
    }
}
