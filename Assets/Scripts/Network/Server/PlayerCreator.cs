using MassiveNet;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCreator : MonoBehaviour {

    public NetView View { get; private set; }

    private Inventory inventory;
    private Equipment equipment;

    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastVel = Vector3.zero;

    void Awake() {
        View = GetComponent<NetView>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();

        inventory.OnItemAdd += OnItemAdded;
        inventory.OnItemRemove += OnItemRemoved;
        equipment.OnEquip += OnEquipped;

        View.OnWriteSync += WriteSync;
        View.OnReadSync += ReadSync;

        View.OnWriteOwnerData += WriteOwnerData;
        View.OnWriteProxyData += WriteInstantiateData;
        View.OnWritePeerData += WriteInstantiateData;
        View.OnWriteCreatorData += WriteOwnerData;

        View.OnReadInstantiateData += ReadInstantiateData;
    }

    RpcTarget WriteSync(NetStream syncStream) {
        if (lastPos == Vector3.zero) return RpcTarget.None;

        syncStream.WriteVector3(transform.position);
        syncStream.WriteVector3(lastVel);

        lastPos = Vector3.zero;

        return RpcTarget.NonControllers;
    }

    private void ReadSync(NetStream syncStream) {
        Vector3 position = syncStream.ReadVector3();
        Quaternion rotation = syncStream.ReadQuaternion();
        Vector3 velocity = syncStream.ReadVector2();
        lastPos = position;
        lastVel = velocity;
        transform.position = position;
        transform.rotation = rotation;
    }
    private void ReadInstantiateData(NetStream stream) {
        transform.position = stream.ReadVector3();
    }

    private void WriteInstantiateData(NetStream stream) {
        string s = LoginServer.GetAccount(View.Controllers[0].Endpoint).baseModel;
        stream.WriteString(s);
        stream.WriteVector3(transform.position);
    }
    private void WriteOwnerData(NetStream stream) {
        string s = LoginServer.GetAccount(View.Controllers[0].Endpoint).baseModel;
        stream.WriteString(s);
        stream.WriteVector3(transform.position);
    }

    private void OnItemAdded(Item item, int amt){
        View.SendReliable("ReceiveAdd", RpcTarget.Controllers, item, amt);
    }
    private void OnItemRemoved(int index, int amt){
        View.SendReliable("ReceiveRemove", RpcTarget.Controllers, index, amt);
    }
    private void OnEquipped(Equip e){
        View.SendReliable("ReceiveEquip", RpcTarget.Controllers, e);
    }
}
