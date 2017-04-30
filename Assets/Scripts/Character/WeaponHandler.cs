using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles all weapon objects to character object
public class WeaponHandler : MonoBehaviour {

    public Weapon currentWeapon {
        get {
            return weaponIndex < weapons.Length && weaponIndex >= 0 ? weapons[weaponIndex] : null;
        }
    }
    public CharacterModel charModel { get; private set; }
    public Weapon[] weapons { get; private set; }

    private Animator anim;
    public int weaponIndex = -1;
    public int nextAvailableIndex { 
        get {
            for (int i = 0; i < weapons.Length; i++){
                if ( weapons[i] == null ) return i;
            }
            return -1;
        }
    }

    void Awake(){
        charModel = GetComponentInChildren<CharacterModel>();
        anim = GetComponent<Animator>();

        weapons = new Weapon[10];
    }
    void Start(){
        AddWeapon(0,"Models/Weapons/Pistols/Handgun");
        AddWeapon(1,"Models/Weapons/Rifles/AK12");
    }

    // Instantiate and place weapon object to hands, Update animator
    public void Equip(int index){
        if ( charModel == null ) return;

        if ( weaponIndex != -1 ){
            currentWeapon.Unequip(charModel);
        }

        this.Log("Equipping index " + index);
        if ( weapons[index] != null ){
            weaponIndex = index;

            Weapon weapon = weapons[weaponIndex];
            weapon.Equip(charModel.rightHand);

            anim.SetInteger(Settings.instance.anim_weapon_type, (int)currentWeapon.weaponType);
        } else {
            anim.SetInteger(Settings.instance.anim_weapon_type, 0);
        }
    }
    // Unequip current weapon
    public void Unequip(){
        if ( weaponIndex != -1 ){
            currentWeapon.Unequip(charModel);
            anim.SetInteger(Settings.instance.anim_weapon_type, 0);
            charModel.transform.localPosition = charModel.originalPos;
            charModel.transform.localEulerAngles = charModel.originalRot;
            weaponIndex = -1;
        }
    }
    // Add weapon to list, return index of equip
    public void AddWeapon(int index, string path){
        if ( index == -1 ) return;

        this.Log("Adding weapon to handler ("+path+") with index of " + index);
        GameObject o = (GameObject) Instantiate(Resources.Load(path));

        Weapon weapon = o.GetComponent<Weapon>();
        weapon.SetAnim(anim);
        weapon.Unequip(charModel);
        weapons[index] = weapon;

        Equip(index);
    }
}
