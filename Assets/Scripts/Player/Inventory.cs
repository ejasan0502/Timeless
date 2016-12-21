using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Inventory : MonoBehaviour {

    public delegate void ItemAdd(Item item, int amt);
    public delegate void ItemRemove(int index, int amt);

    public event ItemAdd OnItemAdd;
    public event ItemRemove OnItemRemove;

    public readonly List<InventoryItem> items = new List<InventoryItem>();

    private void AddItem(Item item, int amt){
        InventoryItem ii = items.Where(i => i.item.id == item.id).FirstOrDefault<InventoryItem>();
        if ( ii != null ){
            ii.amt += amt;
        } else {
            items.Add(new InventoryItem(item,amt));
        }
    }
    private void RemoveItem(int index, int amt){
        if ( index < items.Count ){
            items[index].amt -= amt;
            if ( items[index].amt < 1 ){
                items.RemoveAt(index);
            }
        }
    }

    [NetRPC]
    private void ReceiveAdd(Item item, int amt){
        AddItem(item,amt);
    }
    [NetRPC]
    private void ReceiveRemove(int index, int amt){
        RemoveItem(index,amt);
    }

    public void SendAdd(string id, int amt){
        Item item = ItemDatabase.instance.GetItem(id);

        AddItem(item,amt);
        if ( OnItemAdd != null ) OnItemAdd(item,amt);
    }
    public void SendAdd(Item item, int amt){
        AddItem(item,amt);
        if ( OnItemAdd != null ) OnItemAdd(item,amt);
    }
    public void SendRemove(int index, int amt){
        RemoveItem(index, amt);
        if ( OnItemRemove != null ) OnItemRemove(index, amt);
    }
}
