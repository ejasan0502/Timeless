using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Saves/Loads all items used in game, Organizes all items based on generated ids;
public class ItemManager {
    
    private List<Equip> equips;
    private List<Item> materials;
    private List<Item> resources;

    private static ItemManager _instance;
    public static ItemManager instance {
        get {
            if ( _instance == null )
                _instance = new ItemManager();

            return _instance;
        }
    }

    public ItemManager(){
        equips = new List<Equip>();
        materials = new List<Item>();
        resources = new List<Item>();

        Initialize();
    }

    public Equip GetEquip(string id){
        return equips.Where<Equip>(e => e.id == id).FirstOrDefault();
    }
    public Item GetMaterial(string id){
        return materials.Where<Item>(e => e.id == id).FirstOrDefault();
    }
    public Item GetResource(string id){
        return resources.Where<Item>(e => e.id == id).FirstOrDefault();
    }   

    // For testing purposes, manually fill lists
    private void Initialize(){
        equips = new List<Equip>(){
            new Equip("Hatchet","equip-0",false,ItemType.equip,1f,"Icons/Weapons/Axes/default","Models/Weapons/Tools/Hatchet"),
            new Equip("Handgun","equip-1",false,ItemType.equip,1f,"Icons/Weapons/Pistols/default","Models/Weapons/Pistols/Handgun")
        };
        materials = new List<Item>(){
            new Item("Wood Plank","material-0",true,ItemType.material,0.01f,"Icons/Materials/wood_plank")
        };
        resources = new List<Item>(){
            new Item("Wood","resource-0",true,ItemType.resource,0.01f,"Icons/Resources/wood")
        };
    }
}
