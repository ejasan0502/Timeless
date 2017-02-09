using MassiveNet;
using UnityEngine;

// Client sync
public class PlayerReadSync : MonoBehaviour {

    public Animator anim;
    private NetView view;
    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastVel = Vector3.zero;
    private float lastTime = 0f;
    private Vector3 posDif;
    private Vector3 lastConfirmedPos;
    private Vector3 localLastPos;

    void Awake() {
        view = GetComponent<NetView>();
        view.OnReadSync += ReadSync;
    }
    void Update(){
        //SmoothCorrectPosition();
        //transform.position = transform.position + lastVel * Time.deltaTime;
        if (Time.time - lastTime > 1.2) return;
        SmoothCorrectPosition();
        transform.position = transform.position + lastVel * Time.deltaTime;
        Vector3 vel = transform.position - localLastPos;
        if (vel != Vector3.zero) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 4);
        localLastPos = transform.position;
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
        if ( anim != null ) anim.SetFloat("speed", Mathf.Abs(vel.normalized.x)+Mathf.Abs(vel.normalized.z));
    }

    private void SmoothCorrectPosition(){
        if (Time.time - lastTime > 0.8f) return;
        float dist = Vector3.Distance(transform.position, lastPos);
        if (lastVel.magnitude < 0.2 && dist < 0.5) return;
        transform.position = dist > 10 ? lastPos : Vector3.Lerp(transform.position, transform.position - posDif, Time.deltaTime * 3);
    }
}