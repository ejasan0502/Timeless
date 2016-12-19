using MassiveNet;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCreator : MonoBehaviour {

    public NetView View { get; private set; }

    private Inventory inventory;

    void Awake() {
        View = GetComponent<NetView>();
        inventory = GetComponent<Inventory>();

        inventory.OnItemAdd += OnItemAdded;
        inventory.OnItemRemove += OnItemRemoved;

        View.OnWriteSync += WriteSync;
        View.OnReadSync += ReadSync;

        View.OnWriteOwnerData += WriteOwnerData;
        View.OnWriteProxyData += WriteInstantiateData;
        View.OnWritePeerData += WriteInstantiateData;
        View.OnWriteCreatorData += WriteOwnerData;

        View.OnReadInstantiateData += ReadInstantiateData;
    }

    private Vector3 lastPos = Vector3.zero;
    private Vector2 lastVel = Vector3.zero;

    RpcTarget WriteSync(NetStream syncStream) {
        if (lastPos == Vector3.zero) return RpcTarget.None;

        syncStream.WriteFloat(transform.position.x);
        syncStream.WriteFloat(transform.position.z);
        syncStream.WriteVector2(lastVel);

        lastPos = Vector3.zero;

        return RpcTarget.NonControllers;
    }

    void ReadSync(NetStream syncStream) {
        Vector3 position = syncStream.ReadVector3();
        Quaternion rotation = syncStream.ReadQuaternion();
        Vector2 velocity = syncStream.ReadVector2();
        lastPos = position;
        lastVel = velocity;
        transform.position = position;
        transform.rotation = rotation;
    }
    void ReadInstantiateData(NetStream stream) {
        transform.position = stream.ReadVector3();
    }

    void WriteInstantiateData(NetStream stream) {
        stream.WriteVector3(transform.position);
    }
    void WriteOwnerData(NetStream stream) {
        stream.WriteVector3(transform.position);
    }

    private void OnItemAdded(Item item, int amt){
        View.SendReliable("ReceiveAdd", RpcTarget.Controllers, item.id, amt);
    }
    private void OnItemRemoved(int index, int amt){
        View.SendReliable("ReceiveRemove", RpcTarget.Controllers, index, amt);
    }
}
