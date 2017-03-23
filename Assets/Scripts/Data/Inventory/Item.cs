using UnityEngine;
using System.Reflection;
using System.Collections;

[System.Serializable]
public class Item {

    public string name;
    public string id;
    public bool stackable;
    public ItemType itemType;
    public float weight;
    public string iconPath;
    public string modelPath;

    private Sprite icon = null;
    public Sprite Icon {
        get {
            if ( icon == null ){
                icon = (Sprite) Resources.Load<Sprite>(iconPath);
            }
            return icon;
        }
    }
    public string Description {
        get {
            string s = "";

            s += name +"\n";
            s += itemType + "\n";
            s += weight + " lb\n";

            return s;
        }
    }

    public Item(string name, string id, bool stackable, ItemType itemType, float weight, string iconPath, string modelPath){
        this.name = name;
        this.id = id;
        this.stackable = stackable;
        this.itemType = itemType;
        this.weight = weight;
        this.iconPath = iconPath;
        this.modelPath = modelPath;
    }
    public Item(Item item){
        FieldInfo[] fields1 = GetType().GetFields();
        FieldInfo[] fields2 = item.GetType().GetFields();

        for (int i = 0; i < fields1.Length; i++){
            fields1[i].SetValue(this, fields2[i].GetValue(item));
        }
    }
}
