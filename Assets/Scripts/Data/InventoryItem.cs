using UnityEngine;
using System.Collections;

public class InventoryItem {

    public Item item;
    public int amt;

    public InventoryItem(Item item, int amt){
        this.item = item;
        this.amt = amt;
    }
}
