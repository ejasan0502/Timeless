using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using MassiveNet;

public class Equip : Item {

    public EquipType equipType;
    public EquipStats stats;
    public string modelPath;

    public GameObject Model {
        get {
            return modelPath != "" ? Resources.Load<GameObject>(modelPath) : null;
        }
    }
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
        modelPath = "";
    }
    public Equip(string name, string id, string description, string iconPath, ItemType itemType, EquipType equipType, EquipStats stats, string modelPath){
        this.name = name;
        this.id = id;
        this.description = description;
        this.iconPath = iconPath;
        this.itemType = itemType;

        this.equipType = equipType;
        this.stats = new EquipStats(stats);
        this.modelPath = modelPath;
    }
    public Equip(Equip e){
        name = e.name;
        id = e.id;
        description = e.description;
        iconPath = e.iconPath;
        itemType = ItemType.equip;

        equipType = e.equipType;
        stats = new EquipStats(e.stats);
        modelPath = e.modelPath;
    }
    public Equip(string s){
        string[] args = s.Split(',');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, Global.Parse(fields[i].FieldType, args[i]));
        }
    }
}
