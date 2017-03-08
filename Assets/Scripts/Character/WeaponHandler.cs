using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles all weapon objects to character object
public class WeaponHandler : MonoBehaviour {

    public Weapon currentWeapon { get; private set; }

    private List<Weapon> weapons = new List<Weapon>();

    private CharacterModel charModel;
    private Animator anim;

    void Awake(){
        charModel = GetComponentInChildren<CharacterModel>();
        anim = GetComponent<Animator>();
    }
    void Start(){
        AddWeapon("Models/Weapons/Swords/Morfus");
        AddWeapon("Models/Weapons/Pistols/Handgun");
    }

    // Instantiate and place weapon object to hands, Update animator
    public void Equip(int weaponIndex){
        if ( charModel == null ) return;

        // Remove previous equip
        if ( currentWeapon != null ){
            Destroy(currentWeapon.gameObject);
        }

        Weapon weapon = weapons[weaponIndex];
        weapon.Equip(charModel.rightHand);

        currentWeapon = weapon;
        anim.SetInteger(Settings.instance.anim_weapon_type, (int)currentWeapon.weaponType);
    }
    // Add weapon to list
    public void AddWeapon(string path){
        GameObject o = (GameObject) Instantiate(Resources.Load(path));

        Weapon weapon = o.GetComponent<Weapon>();
        weapon.SetAnim(anim);
        weapon.Unequip(weapon.holster == HolsterType.left ? charModel.leftHolster : weapon.holster == HolsterType.right ? charModel.rightHolster : charModel.backHolster);
        weapons.Add(weapon);
    }
}
