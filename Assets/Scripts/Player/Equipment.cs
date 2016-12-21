using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Equipment : MonoBehaviour {

    private Equip[] equips;

    void Awake(){
        equips = new Equip[Enum.GetValues(typeof(EquipType)).Length];
    }

}
