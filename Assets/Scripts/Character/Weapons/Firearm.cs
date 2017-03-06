using UnityEngine;
using System.Collections;

// Handles pistol weapon object
public class Firearm : Weapon {

    [Header("-Firearm Info-")]
    public bool autoFire;

    [Header("-Object References-")]
    public GameObject decalRef;

    [Header("-Transforms-")]
    public Transform bulletSpawn;
    public Transform bulletForward;

    // Override logic for single click fire
    public override void SinglePrimaryFire(){
        if ( autoFire ) return;

        anim.Play("shooting",1);
        Fire();
    }
    // Override logic for hold click fire
    public override void PrimaryFire(){
        if ( !autoFire ) return;

        anim.SetBool(Settings.instance.anim_primary_attack, true);
        Fire();
    }

    private void Fire(){
        if ( !bulletSpawn ) return;

        RaycastHit hit;
        if ( Physics.Raycast(bulletSpawn.position, bulletForward.position-bulletSpawn.position, out hit, atkRange) ){
            if ( debug ) Debug.DrawRay(bulletSpawn.position, hit.point-bulletSpawn.position, Color.red, 1f);

            Instantiate(decalRef, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
