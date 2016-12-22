using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Equipment : MonoBehaviour {

    public delegate void Equipping(Item item, int amt);

    public event Equipping OnEquip;

    private Equip[] equips;

    void Awake(){
        equips = new Equip[Enum.GetValues(typeof(EquipType)).Length];
    }

    private void Equip(Equip e){
        equips[(int)e.equipType] = e;
    }

}
