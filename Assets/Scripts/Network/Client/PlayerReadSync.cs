using MassiveNet;
using UnityEngine;

// Client sync
public class PlayerReadSync : MonoBehaviour {

    private NetView view;
    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastVel = Vector3.zero;
    private float lastTime = 0f;
    private Vector3 posDif;
    private Vector3 lastConfirmedPos;

    void Awake() {
        view = GetComponent<NetView>();
        view.OnReadSync += ReadSync;
    }
    void Update(){
        SmoothCorrectPosition();
        transform.position = transform.position + lastVel * Time.deltaTime;
    }

    void ReadSync(NetStream syncStream) {
        Vector3 pos = syncStream.ReadVector3();
        Quaternion rot = syncStream.ReadQuaternion();
        Vector3 vel = syncStream.ReadVector3();

        lastPos = pos;
        lastVel = vel;
        transform.rotation = rot;

        if ( Time.time - lastTime > 1.2f ){
            if ( Vector3.Distance(transform.position, lastPos) > 1f ){
                transform.position = lastPos;
            }
        }

        lastTime = Time.time;
        posDif = transform.position - lastPos;
    }

    private void SmoothCorrectPosition(){
        float dist = Vector3.Distance(transform.position, lastPos);
        if (Time.time - lastTime > 0.8f || lastVel.magnitude < 0.2 && dist < 1) return;
        transform.position = dist > 10 ? lastPos : Vector3.Lerp(transform.position, transform.position - posDif, Time.deltaTime*2f);
    }
}