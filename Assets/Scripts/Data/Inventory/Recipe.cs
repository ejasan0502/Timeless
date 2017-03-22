using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recipe {

    public string productId;
    public float craftTime;
    public Dictionary<string,int> itemsRequired;

    public string Description {
        get {
            string s = "";

            s += "Craft Time: " + craftTime + " secs\n\n";
            s += "Required materials:\n";
            foreach (KeyValuePair<string,int> item in itemsRequired){
                s += " - " + item.Value + " " + ItemManager.instance.GetItem(item.Key).name;
            }

            return s;
        }
    }

    public Recipe(string productId, float craftTime, List<string> ids, List<int> amounts){
        this.productId = productId;
        this.craftTime = craftTime;

        itemsRequired = new Dictionary<string,int>();
        for (int i = 0; i < ids.Count; i++){
            if ( i < amounts.Count ){
                itemsRequired.Add(ids[i],amounts[i]);
            }
        }
    }
}
