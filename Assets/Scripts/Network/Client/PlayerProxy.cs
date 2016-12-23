using MassiveNet;
using UnityEngine;

public class PlayerProxy : MonoBehaviour {

    private NetView view;

    void Awake() {
        view = GetComponent<NetView>();

        view.OnReadInstantiateData += Instantiate;
    }

    private void Instantiate(NetStream stream) {
        string baseModel = stream.ReadString();
        if ( baseModel != "" ){
            GameObject o = (GameObject) Instantiate(Resources.Load(baseModel));
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(0f,-1f,0f);
            GetComponent<PlayerReadSync>().anim = o.GetComponent<Animator>();
        }

        transform.position = stream.ReadVector3();
    }

}
