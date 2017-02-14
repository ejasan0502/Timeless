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
    private Animator anim;

    void Awake(){
        equips = new Equip[Enum.GetValues(typeof(EquipType)).Length];
        equipObjs = new GameObject[equips.Length];
        inventory = GetComponent<Inventory>();
    }

    public void Equip(Equip e){
        int index = (int) e.equipType;
        equips[index] = e;

        if ( charModel != null && charModel.nodes[index] != null ){
            if ( equipObjs[index] != null ){
                Destroy(equipObjs[index]);
            }

            equipObjs[index] = (GameObject) Instantiate(e.Model);

            Vector3 pos = equipObjs[index].transform.position;
            Quaternion rot = equipObjs[index].transform.rotation;

            equipObjs[index].transform.SetParent(charModel.nodes[(int)EquipType.primary]);
            equipObjs[index].transform.localPosition = pos;
            equipObjs[index].transform.localRotation = rot;

            foreach (Transform t in equipObjs[index].transform){
                t.gameObject.layer = LayerMask.NameToLayer("Limbs");
            }

            anim.runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load(equips[index].animPath);
            GetComponent<Character>().SetPrimary((WeaponType)Enum.Parse(typeof(WeaponType),equips[index].modelPath.Split('/')[3].ToLower()));
        }
    }

    [NetRPC]
    private void ReceiveEquip(Equip e, NetConnection conn){
        Equip(e);
    }

    public void SetCharModel(CharacterModel cm){
        charModel = cm;
    }
    public void SetAnim(Animator anim){
        this.anim = anim;
    }
    public void SendEquip(int slotIndex){
        if ( slotIndex < inventory.items.Count ){
            Equip e = inventory.items[slotIndex].item.GetAsEquip();
            inventory.SendRemove(slotIndex, 1);
            Equip(e);
            if ( OnEquip != null ) OnEquip(e);
        }
    }
    public string DataToString(){
        string s = "";
        for (int i = 0; i < equips.Length; i++){
            if ( equips[i] != null ){
                if ( i != 0 ){
                    s += ",";
                } 

                s += i + "-" + equips[i].modelPath;
            }
        }
        return s;
    }

    [NetRPC]
    private void EquipRequest(int slotIndex, NetConnection conn){
        SendEquip(slotIndex);
    }
}
