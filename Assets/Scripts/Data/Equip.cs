using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using MassiveNet;

public class Equip : Item {

    public EquipType type;
    public EquipStats stats;

    public override Equip GetAsEquip(){
        return this;
    }

    public Equip(){
        name = "";
        id = "";
        description = "";
        iconPath = "";

        type = EquipType.primary;
        stats = new EquipStats();
    }
    public Equip(Equip e){
        name = e.name;
        id = e.id;
        description = e.description;
        iconPath = e.iconPath;

        type = e.type;
        stats = new EquipStats(e.stats);
    }
    public Equip(string s){
        string[] args = s.Split(',');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            if ( i >= fields.Length ) break;
            
            if ( typeof(EquipType).IsAssignableFrom(fields[i].GetValue(this).GetType()) ){
                fields[i].SetValue(this, (EquipType)Enum.Parse(typeof(EquipType), args[i]));
            } else if ( typeof(EquipStats).IsAssignableFrom(fields[i].GetValue(this).GetType()) ){
                fields[i].SetValue(this, new EquipStats(args[i]));
            } else {
                fields[i].SetValue(this, args[i]);
            }
        }
    }

    public static void SerializeItem(NetStream stream, object instance){
        Item item = (Item)instance;
        stream.WriteString(item.ToString());
    }
    public static object DeserializeItem(NetStream stream){
        Item item = new Item(stream.ReadString());
        return item;
    }

    public override string ToString(){
        FieldInfo[] fields = GetType().GetFields();

        string s = "";
        for (int i = 0; i < fields.Length; i++){
            if ( i != 0 )
                s += ",";
                
            if ( typeof(EquipType).IsAssignableFrom(fields[i].GetValue(this).GetType()) ){
                s += ((EquipType)fields[i].GetValue(this)).ToString();
            } else if ( typeof(EquipStats).IsAssignableFrom(fields[i].GetValue(this).GetType()) ){
                s += ((EquipStats)fields[i].GetValue(this)).ToString();
            } else {
                s += (string)fields[i].GetValue(this);
            }
        }

        return s;
    }
}
