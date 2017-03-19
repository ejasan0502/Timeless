using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles all weapon objects to character object
public class WeaponHandler : MonoBehaviour {

    public Weapon currentWeapon {
        get {
            return weaponIndex < weapons.Count && weaponIndex >= 0 ? weapons[weaponIndex] : null;
        }
    }
    public CharacterModel charModel { get; private set; }

    public List<Weapon> weapons = new List<Weapon>();

    private Animator anim;
    public int weaponIndex = -1;

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
        if ( charModel == null ) return;

        if ( weaponIndex != -1 ){
            currentWeapon.Unequip(charModel);
        }

        weaponIndex = index;
        if ( weaponIndex >= weapons.Count ) weaponIndex = 0;

        Weapon weapon = weapons[weaponIndex];
        weapon.Equip(charModel.rightHand);

        anim.SetInteger(Settings.instance.anim_weapon_type, (int)currentWeapon.weaponType);
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
