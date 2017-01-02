using MassiveNet;
using UnityEngine;

public class PlayerWriteSync : MonoBehaviour {

    private float inputX;
    private float inputZ;
    private Vector3 lastPos;
    private NetView netView;

    void Start() {
        netView = GetComponent<NetView>();
        netView.OnWriteSync += WriteSync;
    }

    private RpcTarget WriteSync(NetStream syncStream) {
        Vector3 velocity = transform.position - lastPos;

        syncStream.WriteVector3(transform.position);
        syncStream.WriteQuaternion(transform.rotation);
        syncStream.WriteVector3(velocity);
        lastPos = transform.position;

        return RpcTarget.All;
    }

}
