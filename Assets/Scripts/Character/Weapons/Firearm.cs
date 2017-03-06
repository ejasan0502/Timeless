using UnityEngine;
using System.Collections;

// Handles pistol weapon object
public class Firearm : Weapon {

    [Header("-Firearm Info-")]
    public bool autoFire;

    [Header("-Object References-")]
    public GameObject decalRef;

    [Header("-Positions-")]
    public Transform bulletSpawn;
    public Transform bulletForward;
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
        Aim();
    }

    // Perform bullet logic
    private void Fire(){
        if ( !bulletSpawn ) return;

        RaycastHit hit;
        if ( Physics.Raycast(bulletSpawn.position, bulletForward.position-bulletSpawn.position, out hit, atkRange) ){
            if ( debug ) Debug.DrawRay(bulletSpawn.position, hit.point-bulletSpawn.position, Color.red, 1f);

            GameObject o = Instantiate(decalRef, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
            Destroy(o, 2f);
        }
    }
    // Perform aiming logic
    private void Aim(){
        // Initialize
        if ( !camTrans ){
            camTrans = GameObject.FindObjectOfType<CameraUpdate>().transform;
            headTrans = camTrans.parent;
            camPos = camTrans.localPosition;
        }

        // Reverse aiming
        aiming = !aiming;

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
