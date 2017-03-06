﻿using UnityEngine;
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
        Equip();
    }

    // Instantiate and place weapon object to hands, Update animator
    public void Equip(){
        if ( charModel == null ) return;

        // Remove previous equip
        if ( currentWeapon != null ){
            Destroy(currentWeapon.gameObject);
        }

        GameObject o = (GameObject) Instantiate(Resources.Load("Models/Weapons/Pistols/Handgun"));

        Weapon weapon = o.GetComponent<Weapon>();
        weapon.Equip(charModel.rightHand);

        currentWeapon = weapon;
        currentWeapon.SetAnim(anim);
        anim.SetInteger(Settings.instance.anim_weapon_type, (int)currentWeapon.weaponType);
    }

}