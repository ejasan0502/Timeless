using UnityEngine;
using System.Net;
using System.Collections;
using MassiveNet;

public class Client : MonoBehaviour {
    private NetConnection server;

    private NetSocket socket;
    private NetViewManager viewManager;
    private NetZoneClient zoneClient;

    void Start() {
        socket = GetComponent<NetSocket>();
        viewManager = GetComponent<NetViewManager>();
        zoneClient = GetComponent<NetZoneClient>();

        zoneClient.OnZoneSetupSuccess += ZoneSetupSuccessful;

        socket.Events.OnDisconnectedFromServer += DisconnectedFromServer;

        socket.StartSocket();
        socket.RegisterRpcListener(this);
    }

    private void ZoneSetupSuccessful(NetConnection zoneServer) {

    }

    private void DisconnectedFromServer(NetConnection serv) {
        viewManager.DestroyViewsServing(serv);
    }
}
