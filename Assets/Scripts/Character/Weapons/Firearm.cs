using UnityEngine;
using System.Collections;

// Handles pistol weapon object
public class Firearm : Weapon {

    [Header("-Firearm Info-")]
    public bool autoFire;
    public float bulletSpread;

    [Header("-Object References-")]
    public GameObject decalRef;
    public GameObject muzzleFlashRef;

    [Header("-Positions-")]
    public Transform bulletSpawn;
    public Vector3 aimPos;

    private bool aiming = false;
    private Transform camTrans;
    private Vector3 camPos;
    private Transform headTrans;

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
    // Override logic for hold click secondary
    public override void SecondaryFire(){
        Aim(true);
    }
    public override void SecondaryFireEnd(){
        Aim(false);
    }

    // Perform bullet logic
    private void Fire(){
        if ( !bulletSpawn ) return;

        // Recoil
        Vector3 direction = bulletSpawn.forward + (Vector3)Random.insideUnitCircle*bulletSpread;

        RaycastHit hit;
        if ( Physics.Raycast(bulletSpawn.position, direction, out hit, atkRange) ){
            if ( debug ) Debug.DrawRay(bulletSpawn.position, hit.point-bulletSpawn.position, Color.red, 1f);

            // Decal
            if ( decalRef ){
                GameObject decal = Instantiate(decalRef, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                Destroy(decal, 2f);
            }

            // Muzzle Flash
            if ( muzzleFlashRef ){
                GameObject muzzleFlash = Instantiate(muzzleFlashRef, bulletSpawn.position, Quaternion.identity, bulletSpawn) as GameObject;
                Destroy(muzzleFlash, 0.2f);
            }
        }
    }
    // Perform aiming logic
    private void Aim(bool b){
        // Initialize
        if ( !camTrans ){
            camTrans = GameObject.FindObjectOfType<CameraUpdate>().transform;
            headTrans = camTrans.parent;
            camPos = camTrans.localPosition;
        }

        // Reverse aiming
        aiming = b;

        // Adjust transforms
        if ( aiming ){
            camTrans.SetParent(transform);
            camTrans.localPosition = aimPos;
        } else {
            camTrans.SetParent(headTrans);
            camTrans.localPosition = camPos;
        }
    }
}
