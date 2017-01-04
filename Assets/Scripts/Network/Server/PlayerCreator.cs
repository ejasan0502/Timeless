using MassiveNet;
using UnityEngine;
using System.Collections.Generic;

// Server sync
public class PlayerCreator : MonoBehaviour {

    public NetView View { get; private set; }

    private Inventory inventory;
    private Equipment equipment;
    private Character character;

    void Awake() {
        View = GetComponent<NetView>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
        character = GetComponent<Character>();
        character.id = IDManager.GenerateId();

        inventory.OnItemAdd += OnItemAdded;
        inventory.OnItemRemove += OnItemRemoved;
        equipment.OnEquip += OnEquipped;

        View.OnWriteOwnerData += WriteOwnerData;
        View.OnWriteProxyData += WriteInstantiateData;
        View.OnWritePeerData += WriteInstantiateData;
        View.OnWriteCreatorData += WriteOwnerData;

        View.OnReadInstantiateData += ReadInstantiateData;
    }

    private void ReadInstantiateData(NetStream stream) {
        transform.position = stream.ReadVector3();
    }

    private void WriteInstantiateData(NetStream stream) {
        string s = LoginServer.GetAccount(View.Controllers[0].Endpoint).baseModel;
        stream.WriteString(s);
        stream.WriteString(equipment.DataToString());
        stream.WriteVector3(transform.position);
        stream.WriteString(character.id);
    }
    private void WriteOwnerData(NetStream stream) {
        string s = LoginServer.GetAccount(View.Controllers[0].Endpoint).baseModel;
        stream.WriteString(s);
        stream.WriteVector3(transform.position);
        stream.WriteString(character.id);
    }

    private void OnItemAdded(Item item, int amt){
        View.SendReliable("ReceiveAdd", RpcTarget.Controllers, item, amt);
    }
    private void OnItemRemoved(int index, int amt){
        View.SendReliable("ReceiveRemove", RpcTarget.Controllers, index, amt);
    }
    private void OnEquipped(Equip e){
        View.SendReliable("ReceiveEquip", RpcTarget.Controllers, e);
        View.SendReliable("UpdateEquip", RpcTarget.NonControllers, (int)e.equipType, e.modelPath);
    }
}
