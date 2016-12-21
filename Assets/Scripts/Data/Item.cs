using UnityEngine;
using System.Net;
using System.Reflection;
using System.Collections;
using MassiveNet;

public class Item {

    public string name;
    public string id;
    public string description;
    public string iconPath;

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
    }
    public Item(string name, string id, string description, string icon){
        this.name = name;
        this.id = id;
        this.description = description;
        this.iconPath = icon;
    }
    public Item(Item item){
        FieldInfo[] fields1 = GetType().GetFields();
        FieldInfo[] fields2 = item.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields1[i].SetValue(this, (string)fields2[i].GetValue(item));
        }
    }
    public Item(string s){
        string[] args = s.Split(',');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            if ( i >= fields.Length ) break;

            fields[i].SetValue(this, (string)args[i]);
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

            s += (string)fields[i].GetValue(this);
        }

        return s;
    }
}
