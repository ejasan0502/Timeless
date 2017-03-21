using UnityEngine;
using System.Collections;

public class InventoryItem {
    public Item item;
    public int amount;

    public InventoryItem(Item item, int amount){
        this.item = item;
        this.amount = amount;
    }
}
