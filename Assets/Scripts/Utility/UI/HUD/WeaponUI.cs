using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Handles UI for fire arms
public class WeaponUI : UI {
    
    public Text ammoText;

    private WeaponHandler weaponHandler;

    void Start(){
        weaponHandler = GameObject.FindWithTag("Player").GetComponent<WeaponHandler>();
    }
    void FixedUpdate(){
        if ( weaponHandler && ammoText ){
            if ( weaponHandler.currentWeapon && weaponHandler.currentWeapon.isFirearm ){
                Firearm weapon = weaponHandler.currentWeapon as Firearm;
                ammoText.text = weapon.clipSize + " | " + weapon.carryingAmmo;
            } else if ( ammoText.text != "" ){
                ammoText.text = "";
            }
        }
    }

}
