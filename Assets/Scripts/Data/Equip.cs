using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using MassiveNet;

public class Equip : Item {

    public EquipType equipType;
    public EquipStats stats;

    public override Equip GetAsEquip(){
        return this;
    }

    public Equip(){
        name = "";
        id = "";
        description = "";
        iconPath = "";
        itemType = ItemType.equip;

        equipType = EquipType.primary;
        stats = new EquipStats();
    }
    public Equip(string name, string id, string description, string iconPath, ItemType itemType, EquipType equipType, EquipStats stats){

    }
    public Equip(Equip e){
        name = e.name;
        id = e.id;
        description = e.description;
        iconPath = e.iconPath;
        itemType = ItemType.equip;

        equipType = e.equipType;
        stats = new EquipStats(e.stats);
    }
    public Equip(string s){
        string[] args = s.Split(',');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            if ( i >= fields.Length ) break;
            
            fields[i].SetValue(this, Global.Parse(fields[i].FieldType, args[i]));
        }
    }

    public override string ToString(){
        FieldInfo[] fields = GetType().GetFields();

        string s = "";
        for (int i = 0; i < fields.Length; i++){
            if ( i != 0 )
                s += ",";
                
            if ( fields[i].FieldType == typeof(EquipType) ){
                s += ((EquipType)fields[i].GetValue(this)).ToString();
            } else if ( fields[i].FieldType == typeof(EquipStats) ){
                s += ((EquipStats)fields[i].GetValue(this)).ToString();
            } else {
                s += (string)fields[i].GetValue(this);
            }
        }

        return s;
    }
}
