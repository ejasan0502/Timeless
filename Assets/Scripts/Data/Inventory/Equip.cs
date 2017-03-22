using UnityEngine;
using System.Collections;

public class Equip : Item {

    public string modelPath;

    public Equip(string name, string id, bool stackable, ItemType itemType, float weight, string iconPath, string modelPath) : 
        base(name, id, stackable, itemType, weight, iconPath) {
        this.modelPath = modelPath;
    }

}
