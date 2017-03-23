using UnityEngine;
using System.Collections;

public class ItemBlock : Item {

    public float durability;

    public ItemBlock(string name, string id, bool stackable, ItemType itemType, float weight, string iconPath, string modelPath, float durability) : 
        base (name, id, stackable, itemType, weight, iconPath, modelPath){
        this.durability = durability;
    }

}
