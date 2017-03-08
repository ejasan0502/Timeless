using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles all weapon objects to character object
public class WeaponHandler : MonoBehaviour {

    public Weapon currentWeapon {
        get {
            return weaponIndex < weapons.Count ? weapons[weaponIndex] : null;
        }
    }

    private List<Weapon> weapons = new List<Weapon>();

    private CharacterModel charModel;
    private Animator anim;
    private int weaponIndex = -1;

    void Awake(){
        charModel = GetComponentInChildren<CharacterModel>();
        anim = GetComponent<Animator>();
    }
    void Start(){
        AddWeapon("Models/Weapons/Rifles/AK12");
        AddWeapon("Models/Weapons/Swords/Morfus");
        AddWeapon("Models/Weapons/Pistols/Handgun");
    }

    // Instantiate and place weapon object to hands, Update animator
    public void Equip(int index){
        if ( charModel == null && weaponIndex < weapons.Count ) return;

        if ( weaponIndex != -1 ){
            currentWeapon.Unequip(charModel);
        }

        weaponIndex = index;

        Weapon weapon = weapons[weaponIndex];
        weapon.Equip(charModel.rightHand);

        anim.SetInteger(Settings.instance.anim_weapon_type, (int)currentWeapon.weaponType);
    }
    // Add weapon to list
    public void AddWeapon(string path){
        GameObject o = (GameObject) Instantiate(Resources.Load(path));

        Weapon weapon = o.GetComponent<Weapon>();
        weapon.SetAnim(anim);
        weapon.Unequip(charModel);
        weapons.Add(weapon);

        if ( weaponIndex == -1 ){
            Equip(0);
        }
    }
}
