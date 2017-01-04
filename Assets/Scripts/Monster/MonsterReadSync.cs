using UnityEngine;
using System.Collections;
using MassiveNet;

public class MonsterReadSync : MonoBehaviour {

    private NetView view;
    private Character character;

    void Awake(){
        view = GetComponent<NetView>();
        character = GetComponent<Character>();
    }
    void Start(){
        view.OnReadInstantiateData += OnReadInstantiate;
        view.OnReadSync += OnReadSync;
    }

    private void OnReadInstantiate(NetStream stream){
        Vector3 pos = stream.ReadVector3();
        string id = stream.ReadString();

        transform.position = pos;
        character.id = id;
    }
    private void OnReadSync(NetStream stream){
        Vector3 pos = stream.ReadVector3();
        transform.position = pos;
    }
}
