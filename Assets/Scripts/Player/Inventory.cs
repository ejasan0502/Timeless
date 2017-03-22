using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Manages inventory based on weight and effects character movement speed
public class Inventory : MonoBehaviour {

    public bool debug = false;

    public float Weight {
        get {
            return weight;
        }
    }
    public List<InventoryItem> Items {
        get {
            return inventoryItems;
        }
    }

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private float weight = 0f;

    private Character character;

    void Awake(){
        character = GetComponent<Character>();
    }
    void Start(){
        CalculateWeight();
    }

    // Check if inventory contains given item id and amount
    public bool HasItem(string id, int amount){
        InventoryItem ii = GetInventoryItem(id);
        return ii != null && ii.amount >= amount;
    }
    // Check if inventory contains given specific item and amount
    public bool HasItem(Item item, int amount){
        InventoryItem ii = GetInventoryItem(item);
        if ( ii != null ){
            if ( ii.amount >= amount ){
                return true;
            }
        }
        return false;
    }
    // Add specific item into inventory with amount
    public void AddItem(Item item, int amount){
        if ( debug ) Debug.Log("Gained " + amount + " " + item.name);

        InventoryItem ii = GetInventoryItem(item);
        if ( ii != null && ii.item.stackable ){
            ii.amount += amount;
        } else {
            if ( item.stackable ){
                inventoryItems.Add(new InventoryItem(item, amount));
            } else {
                for (int i = 0; i < amount; i++){
                    inventoryItems.Add(new InventoryItem(item, 1));
                }
            }
        }
        CalculateWeight();
    }
    // Add generic item into inventory based on item id with amount
    public void AddItem(string id, int amount){
        if ( id == "" ) return;

        InventoryItem ii = GetInventoryItem(id);
        if ( ii != null && ii.item.stackable ){
            if ( debug ) Debug.Log("Gained " + amount + " " + ii.item.name);
            ii.amount += amount;
        } else {
            Item item = ItemManager.instance.GetItem(id);
            if ( debug ) Debug.Log("Gained " + amount + " " + item.name);
            if ( item.stackable ){
                inventoryItems.Add(new InventoryItem(item, amount));
            } else {
                for (int i = 0; i < amount; i++){
                    inventoryItems.Add(new InventoryItem(item, 1));
                }
            }
        }
        CalculateWeight();
    }
    // Remove item from inventory with amount
    public void RemoveItem(Item item, int amount){
        InventoryItem ii = GetInventoryItem(item);
        if ( ii != null ){
            ii.amount -= amount;
            if ( ii.amount < 1 ){
                inventoryItems.Remove(ii);
            }
        }
    }
    // Remove item from inventory based on item id and amount
    public void RemoveItem(string id, int amount){
        int currentAmount = amount;

        while ( currentAmount > 0 ){
            InventoryItem ii = GetInventoryItem(id);
            if ( ii != null ){
                currentAmount -= ii.amount;
                if ( currentAmount < 0 ){
                    ii.amount = Mathf.Abs(currentAmount);
                } else if ( currentAmount >= 0 ){
                    inventoryItems.Remove(ii);
                }
            } else {
                Debug.Log("RemoveItem was called with an amount higher than whats in inventory...");
                currentAmount = 0;
            }
        }
    }
    // Get inventory item from inventory based on index
    public InventoryItem GetInventoryItem(int index){
        return inventoryItems[index];
    }

    // Get inventory item from inventory based on item
    private InventoryItem GetInventoryItem(Item item){
        return inventoryItems.Where<InventoryItem>(ii => ii.item == item).FirstOrDefault();
    }
    // Get inventory item from inventory based on item id
    private InventoryItem GetInventoryItem(string id){
        return inventoryItems.Where<InventoryItem>(ii => ii.item.id == id).FirstOrDefault();
    }
    // Calculate the total weight from all items in inventory
    private void CalculateWeight(){
        if ( character == null ) return;

        weight = 0f;

        foreach (InventoryItem ii in inventoryItems){
            weight += ii.item.weight*ii.amount;
        }

        if ( character != null ){
            character.currentCharStats.weight = weight;
        }
    }
}
