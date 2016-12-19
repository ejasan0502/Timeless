using UnityEngine;
using System.Collections;

public class Item {

    public string name;
    public string id;
    public string description;
    public Sprite icon;

    public Item(string name, string id, string description, Sprite icon){
        this.name = name;
        this.id = id;
        this.description = description;
        this.icon = icon;
    }
}
