using MassiveNet;
using UnityEngine;

public class PlayerWriteSync : MonoBehaviour {

    private float inputX;
    private float inputZ;
    private Vector3 lastPos;
    private NetView netView;
    private CharacterController cc;

    void Awake(){
        cc = GetComponent<CharacterController>();
    }
    void Start() {
        netView = transform.parent.GetComponent<NetView>();
        netView.OnWriteSync += WriteSync;
    }

    private RpcTarget WriteSync(NetStream syncStream) {
        Vector3 position = transform.position+cc.center;
        Vector3 velocity = position - lastPos;

        syncStream.WriteVector3(position);
        syncStream.WriteQuaternion(transform.rotation);
        syncStream.WriteVector3(velocity);
        lastPos = position;

        return RpcTarget.Server;
    }

}
