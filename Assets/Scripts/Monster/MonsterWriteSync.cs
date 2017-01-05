using UnityEngine;
using System.Collections;
using MassiveNet;

public class MonsterWriteSync : MonoBehaviour {

    private NetView view;
    private Character character;

    void Awake(){
        view = GetComponent<NetView>();
        character = GetComponent<Character>();
        character.id = IDManager.GenerateId();

        view.OnWriteProxyData += OnWriteInstantiate;
        view.OnWriteCreatorData += OnWriteInstantiate;
        view.OnWritePeerData += OnWriteInstantiate;
        view.OnWriteSync += OnWriteSync;

        view.OnReadInstantiateData += OnReadInstantiate;
    }
    
    private void OnReadInstantiate(NetStream stream){
        Vector3 pos = stream.ReadVector3();
        string id = stream.ReadString();

        transform.position = pos;
        character.id = id;
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
