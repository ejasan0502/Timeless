using UnityEngine;
using System.Collections;
using MassiveNet;

public class MonsterWriteSync : MonoBehaviour {

    private NetView view;
    private Character character;

    void Awake(){
        view = GetComponent<NetView>();
        character = GetComponent<Character>();
    }
    void Start(){
        view.OnWriteProxyData += OnWriteInstantiate;
        view.OnWriteSync += OnWriteSync;
    }

    private void OnWriteInstantiate(NetStream stream){
        stream.WriteVector3(transform.position);
        stream.WriteString(character.id);
    }
    private RpcTarget OnWriteSync(NetStream stream){
        stream.WriteVector3(transform.position);
        return RpcTarget.NonControllers;
    }
}
