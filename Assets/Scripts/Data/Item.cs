using UnityEngine;
using System;
using System.Net;
using System.Xml;
using System.Reflection;
using System.Collections;
using MassiveNet;

public class Item {

    public string name;
    public string id;
    public string description;
    public string iconPath;
    public ItemType itemType;

    public Sprite icon {
        get {
            return Resources.Load<Sprite>(iconPath) ?? null;
        }
    }

    public virtual Equip GetAsEquip(){
        return null;
    }

    public Item(){
        name = "";
        id = "";
        description = "";
        iconPath = "Icons/default";
        itemType = ItemType.item;
    }
    public Item(string name, string id, string description, string icon, ItemType itemType){
        this.name = name;
        this.id = id;
        this.description = description;
        this.iconPath = icon;
        this.itemType = itemType;
    }
    public Item(Item item){
        FieldInfo[] fields1 = GetType().GetFields();
        FieldInfo[] fields2 = item.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            if ( typeof(ItemType).IsAssignableFrom(fields2[i].GetValue(item).GetType()) ){
                fields1[i].SetValue(this, (ItemType)fields2[i].GetValue(item));
            } else
                fields1[i].SetValue(this, fields2[i].GetValue(item));
        }
    }
    public Item(string s){
        string[] args = s.Split(',');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            fields[i].SetValue(this, Global.Parse(fields[i].FieldType,args[i]));
        }
    }

    public static void Serialize(NetStream stream, object instance){
        string s = "";

        Item item = (Item)instance;
        if ( item.itemType == ItemType.equip ){
            s = item.GetAsEquip().ToString();
        } else {
            s = item.ToString();
        }

        stream.WriteString(s);
    }
    public static object Deserialize(NetStream stream){
        string s = stream.ReadString();

        string[] args = s.Split('=');
        if ( args[0] == "equip" ){
            return new Equip(args[1]);
        } else if ( args[0] == "usable" ){
            return null;
        } else {
            return new Item(args[1]);
        }
    }

    public override string ToString(){
        FieldInfo[] fields = GetType().GetFields();

        string s = "";
        if ( itemType == ItemType.equip ){
            s = "equip";
        } else if ( itemType == ItemType.usable ){
            s = "usable";
        } else {
            s = "item";
        }
        s += "=";

        for (int i = 0; i < fields.Length; i++){
            if ( i != 0 )
                s += ",";

            if ( fields[i].FieldType == typeof(EquipType) ){
                s += ((EquipType)fields[i].GetValue(this)).ToString();
            } else if ( fields[i].FieldType == typeof(EquipStats) ){
                s += ((EquipStats)fields[i].GetValue(this)).ToString();
            } else if ( fields[i].FieldType == typeof(ItemType) ){
                s += ((ItemType)fields[i].GetValue(this)).ToString();
            } else {
                s += fields[i].GetValue(this);
            }
        }
        return s;
    }
}
