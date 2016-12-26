using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Equipment : MonoBehaviour {

    public delegate void Equipping(Equip e);

    public event Equipping OnEquip;

    private Equip[] equips;
    private GameObject[] equipObjs;
    private CharacterModel charModel = null;
    private Inventory inventory;

    void Awake(){
        equips = new Equip[Enum.GetValues(typeof(EquipType)).Length];
        equipObjs = new GameObject[equips.Length];
        inventory = GetComponent<Inventory>();
    }

    private void Equip(Equip e){
        int index = (int) e.equipType;
        equips[index] = e;

        if ( charModel != null && charModel.nodes[index] != null ){
            if ( equipObjs[index] != null ){
                Destroy(equipObjs[index]);
            }

            equipObjs[index] = (GameObject) Instantiate(e.Model, charModel.nodes[index]);
            equipObjs[index].transform.localPosition = Vector3.zero;
        }
    }

    [NetRPC]
    private void ReceiveEquip(Equip e, NetConnection conn){
        Equip(e);
    }

    public void SetCharModel(CharacterModel cm){
        charModel = cm;
    }
    public void SendEquip(int slotIndex){
        if ( slotIndex < inventory.items.Count ){
            Equip e = inventory.items[slotIndex].item.GetAsEquip();
            inventory.SendRemove(slotIndex, 1);
            Equip(e);
            if ( OnEquip != null ) OnEquip(e);
        }
    }

    [NetRPC]
    private void EquipRequest(int slotIndex, NetConnection conn){
        SendEquip(slotIndex);
    }
}
