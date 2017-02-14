using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class ItemDatabase {

    private List<Item> items = new List<Item>(){
        new Equip("Sword", "sword-0", "This is a sword.", null, ItemType.equip, EquipType.primary, new EquipStats(1f), "Models/Equipment/Weapons/Swords/sword", "Animators/swordAndShield"),
        new Equip("AK47", "rifle-0", "This is an ak47.", null, ItemType.equip, EquipType.primary, new EquipStats(1f), "Models/Equipment/Weapons/Guns/Rifles/Ak47", "Animators/rifle")
    }; 

    private static ItemDatabase _instance;
    public static ItemDatabase instance {
        get {
            if ( _instance == null ){
                _instance = new ItemDatabase();
            }
            return _instance;
        }
    }

    public ItemDatabase(){

    }

    public Item GetItem(string id){
        return items.Where(i => i.id == id).FirstOrDefault<Item>() ?? null;
    }
}
