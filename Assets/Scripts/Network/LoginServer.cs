using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

[RequireComponent(typeof(NetSocket))] 
public class LoginServer : MonoBehaviour {
    
    public delegate void LoginSuccess(NetConnection conn, Account acc);

    public event LoginSuccess OnLoginSuccess;

    private NetSocket socket;
    private readonly List<Account> accounts = new List<Account>();
    private readonly Dictionary<ulong, Account> sessions = new Dictionary<ulong, Account>();
    private readonly Dictionary<IPEndPoint, ulong> sessionLookup = new Dictionary<IPEndPoint, ulong>();


    void Awake(){
        socket = GetComponent<NetSocket>();

        socket.RegisterRpcListener(this);
        socket.Events.OnClientDisconnected += EndSession;
    }

    public bool SessionValid(NetConnection connection, ulong token) {
        return sessionLookup.ContainsKey(connection.Endpoint) && sessionLookup[connection.Endpoint] == token;
    }

    public bool TryGetAccount(NetConnection connection, out Account account) {
        if (!sessionLookup.ContainsKey(connection.Endpoint)) {
            account = null;
            return false;
        }
        account = sessions[sessionLookup[connection.Endpoint]];
        return true;
    }

    [NetRPC]
    private void LoginRequest(string username, string password, NetConnection connection) {
        foreach (Account account in accounts) {
            if ( account.username != username && account.email != username ) continue;
            if (sessions.ContainsValue(account)) SendAlreadyLoggedIn(connection);
            else if (account.password == password) SendLoginSuccess(account, connection);
            else SendBadCredentials(connection);
            return;
        }
        SendBadCredentials(connection);
    }

    [NetRPC]
    private void CreateAccountRequest(string email, string username, string password, NetConnection connection) {
        foreach (Account account in accounts) {
            if (account.username != username) continue;
            SendEmailDuplicate(connection);
            return;
        }
        ulong randId = NetMath.RandomUlong();
        var newAcc = new Account(email, username, password);
        accounts.Add(newAcc);
        SendLoginSuccess(newAcc, connection);
    }

    private ulong CreateSession(Account account, NetConnection connection) {
        ulong sessionToken = NetMath.RandomUlong();
        sessions.Add(sessionToken, account);
        sessionLookup.Add(connection.Endpoint, sessionToken);
        return sessionToken;
    }

    private void EndSession(NetConnection connection) {
        if (!sessionLookup.ContainsKey(connection.Endpoint)) return;
        ulong token = sessionLookup[connection.Endpoint];
        sessions.Remove(token);
        sessionLookup.Remove(connection.Endpoint);
    }

    private void SendEmailDuplicate(NetConnection connection) {
        socket.Send("EmailDuplicateResponse", connection);
    }

    private void SendLoginSuccess(Account account, NetConnection connection) {
        ulong sessionToken = CreateSession(account, connection);
        socket.Send("LoginSuccessResponse", connection, sessionToken);
        if (OnLoginSuccess != null) OnLoginSuccess(connection, account);
    }

    private void SendAlreadyLoggedIn(NetConnection connection) {
        socket.Send("AlreadyLoggedInResponse", connection);
    }

    private void SendBadCredentials(NetConnection connection) {
        socket.Send("BadCredentialsResponse", connection);
    }
}
