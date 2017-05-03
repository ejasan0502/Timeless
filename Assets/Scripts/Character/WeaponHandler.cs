using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles all weapon objects to character object
public class WeaponHandler : MonoBehaviour {

    public const int MaxWeaponEquipped = 2;
    public const int MaxWeaponCount = 10;

    public string[] starterWeapons;

    public List<Weapon> currentWeapons { get; private set; }
    public CharacterModel charModel { get; private set; }
    public Weapon[] weapons;

    private Animator anim;
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

        weapons = new Weapon[MaxWeaponCount];
        currentWeapons = new List<Weapon>();
    }
    void Start(){
        for (int i = 0; i < starterWeapons.Length; i++){
            AddWeapon(i,starterWeapons[i]);
        }
    }

    // Instantiate and place weapon object to hands, Update animator
    public void Equip(int index){
        if ( charModel == null ) return;

        if ( currentWeapons.Count > 0 ){
            if ( !currentWeapons[0].oneHanded ){
                currentWeapons[0].Unequip(charModel);

                currentWeapons = new List<Weapon>();
            }
        }

        this.Log("Equipping index " + index);
        if ( weapons[index] != null ){
            Weapon weapon = weapons[index];
            if ( currentWeapons.Count > 0 ){
                // Character already has a weapon equipped
                if ( currentWeapons[0].weaponType == weapon.weaponType ){
                    weapon.Equip(charModel.leftHand);

                    if ( weapon.isMelee ){
                        anim.SetInteger(Settings.instance.anim_weapon_type, (int)EquipType.dualMelee);
                    } else {
                        anim.SetInteger(Settings.instance.anim_weapon_type, (int)EquipType.dualPistol);
                    }
                } else {
                    if ( currentWeapons[0].isFirearm ){
                        weapon.Equip(charModel.rightHand);
                        currentWeapons[0].Equip(charModel.leftHand);
                    } else {
                        weapon.Equip(charModel.leftHand);
                    }   

                    anim.SetInteger(Settings.instance.anim_weapon_type, (int)EquipType.meleeAndPistol);
                }
            } else {
                // Character does not have a weapon equipped
                weapon.Equip(charModel.rightHand);
                anim.SetInteger(Settings.instance.anim_weapon_type, (int)weapon.weaponType);
            }

            currentWeapons.Add(weapon);
        } else {
            anim.SetInteger(Settings.instance.anim_weapon_type, 0);
        }
    }
    // Unequip current weapon
    //public void Unequip(){
    //    if ( weaponIndex != -1 ){
    //        currentWeapon.Unequip(charModel);
    //        anim.SetInteger(Settings.instance.anim_weapon_type, 0);
    //        charModel.transform.localPosition = charModel.originalPos;
    //        charModel.transform.localEulerAngles = charModel.originalRot;
    //        weaponIndex = -1;
    //    }
    //}
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
