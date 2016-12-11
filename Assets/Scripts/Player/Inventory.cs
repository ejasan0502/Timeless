using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Inventory : MonoBehaviour {

    public delegate void ItemAdded(InventoryItem ii);
    public delegate void ItemRemoved(InventoryItem ii);

    public readonly List<InventoryItem> items = new List<InventoryItem>();

    private NetView netView;

    void Awake(){
        netView = GetComponent<NetView>();
    }

    public void AddItem(Item item, int amt){
        InventoryItem ii = items.Where(i => i.item.id == item.id).FirstOrDefault<InventoryItem>();
        if ( ii != null ){
            ii.amt += amt;
        } else {
            items.Add(new InventoryItem(item,amt));
        }
    }
    public void RemoveItem(int index, int amt){
        if ( index < items.Count ){
            items[index].amt -= amt;
            if ( items[index].amt < 1 ){
                items.RemoveAt(index);
            }
        }
    }

}
