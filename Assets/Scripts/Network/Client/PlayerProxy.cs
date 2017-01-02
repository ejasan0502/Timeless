using MassiveNet;
using UnityEngine;

public class PlayerProxy : MonoBehaviour {

    private NetView view;
    private CharacterModel charModel;

    void Awake() {
        view = GetComponent<NetView>();

        view.OnReadInstantiateData += Instantiate;
    }

    private void Instantiate(NetStream stream) {
        string baseModel = stream.ReadString();
        string equipModels = stream.ReadString();
        Vector3 pos = stream.ReadVector3();

        if ( baseModel != "" ){
            GameObject o = (GameObject) Instantiate(Resources.Load(baseModel));
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(0f,-1f,0f);
            charModel = o.GetComponent<CharacterModel>();

            if ( equipModels != "" ){
                string[] args = equipModels.Split(',');
                for (int i = 0; i < args.Length; i++){
                    string[] vals = args[i].Split('-');
                    UpdateEquip(int.Parse(vals[0]), vals[1], null);
                }
            }
        }

        transform.position = pos;
    }

    [NetRPC]
    private void UpdateEquip(int index, string modelPath, NetConnection conn){
        if ( charModel != null && charModel.nodes[index] != null ){
            if ( charModel.nodes[index].childCount > 0 ){
                Destroy(charModel.nodes[index].GetChild(0).gameObject);
            }

            GameObject o = (GameObject) Instantiate(Resources.Load(modelPath));

            Quaternion rot = o.transform.rotation;
            o.transform.SetParent(charModel.nodes[index]);
            o.transform.localPosition = Vector3.zero;
            o.transform.localRotation = rot;
        }
    }

}
