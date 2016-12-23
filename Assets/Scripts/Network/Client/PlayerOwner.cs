using MassiveNet;
using UnityEngine;

public class PlayerOwner : MonoBehaviour {

    public Inventory inventory;

    private NetView view;

    void Awake() {
        view = GetComponent<NetView>();
        inventory = GetComponent<Inventory>();

        tag = "Player";

        view.OnReadInstantiateData += Instantiate;
    }

    void Instantiate(NetStream stream) {
        string baseModel = stream.ReadString();
        if ( baseModel != "" ){
            GameObject o = (GameObject) Instantiate(Resources.Load(baseModel));
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(0f,-1f,0f);
            GetComponent<PlayerInput>().SetAnim(o.GetComponent<Animator>());
        }

        Vector3 pos = stream.ReadVector3();
        if (transform.position != Vector3.zero && Vector3.Distance(transform.position, pos) < 5) return;
        transform.position = pos;
    }

}
