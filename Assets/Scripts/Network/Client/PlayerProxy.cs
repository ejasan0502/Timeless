using MassiveNet;
using UnityEngine;

public class PlayerProxy : MonoBehaviour {

    private NetView view;
    private Vector3 lastPos;
    private Vector3 lastVel;

    void Awake() {
        view = GetComponent<NetView>();

        view.OnReadInstantiateData += Instantiate;
    }

    private void Instantiate(NetStream stream) {
        transform.position = stream.ReadVector3();
    }

}
