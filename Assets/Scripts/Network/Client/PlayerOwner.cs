using MassiveNet;
using UnityEngine;

public class PlayerOwner : MonoBehaviour {

    public Inventory inventory;
    public Equipment equipment;

    public NetView view { get; private set; }

    void Awake() {
        view = GetComponent<NetView>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();

        tag = "Player";

        view.OnReadInstantiateData += Instantiate;
    }

    void Instantiate(NetStream stream) {
        string baseModel = stream.ReadString();
        Vector3 pos = stream.ReadVector3();
        string id = stream.ReadString();

        if ( baseModel != "" ){
            GameObject o = (GameObject) Instantiate(Resources.Load(baseModel));
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(0f,-1f,0f);
            GetComponent<Equipment>().SetCharModel(o.GetComponent<CharacterModel>());
            GetComponent<Character>().SetAnim(o.GetComponent<Animator>());
            GetComponent<Character>().id = id;
        }

        if (transform.position != Vector3.zero && Vector3.Distance(transform.position, pos) < 5) return;
        transform.position = pos;
    }

}
