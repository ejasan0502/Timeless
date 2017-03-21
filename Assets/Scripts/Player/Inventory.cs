using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Manages player inventory based on weight and effects player movement speed
public class Inventory : MonoBehaviour {

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

    public void AddItem(Item item, int amount){
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
    public void RemoveItem(Item item, int amount){
        InventoryItem ii = GetInventoryItem(item);
        if ( ii != null ){
            ii.amount -= amount;
            if ( ii.amount < 1 ){
                inventoryItems.Remove(ii);
            }
        }
    }

    private InventoryItem GetInventoryItem(Item item){
        return inventoryItems.Where<InventoryItem>(ii => ii.item.id == item.id).FirstOrDefault();
    }
    private void CalculateWeight(){
        weight = 0f;

        foreach (InventoryItem ii in inventoryItems){
            weight += ii.item.weight*ii.amount;
        }

        if ( character != null ){
            character.currentCharStats.weight = weight;
        }
    }
}
