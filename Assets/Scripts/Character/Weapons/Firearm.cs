using UnityEngine;
using System.Collections;

// Handles pistol weapon object
public class Firearm : Weapon {

    [Header("-Firearm Info-")]
    public bool autoFire;
    public float bulletSpread;
    public int clipSize;
    public int maxClipSize;
    public int carryingAmmo;

    [Header("-Object References-")]
    public GameObject decalRef;
    public GameObject muzzleFlashRef;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [Header("-Positions-")]
    public Transform bulletSpawn;
    public Vector3 aimPos;

    private bool aiming = false;
    private Transform camTrans;
    private Transform headTrans;

    // Override bool to true since weapon is firearm
    public override bool isFirearm {
        get {
            return true;
        }
    }

    void Start(){
        camTrans = Camera.main.transform;
        headTrans = camTrans.parent;
    }

    // Single fire on button down
    public override void SinglePrimaryFire(){
        if ( autoFire || anim.GetBool(Settings.instance.anim_reload) ) return;

        anim.Play("shooting",1);
        Fire();
    }
    // Hold fire on button held
    public override void PrimaryFire(){
        if ( !autoFire || anim.GetBool(Settings.instance.anim_reload) ) return;

        anim.SetInteger(Settings.instance.anim_attack, 1);
        Fire();
    }
    // Aiming
    public override void SecondaryFire(){
        if ( anim.GetBool(Settings.instance.anim_reload) ) return;

        Aim(true);
    }
    // Aiming end
    public override void SecondaryFireEnd(){
        Aim(false);
    }
    // Reloading
    public override void AltFire(){
        if ( clipSize == maxClipSize || carryingAmmo <= 0 ) return;

        Aim(false);

        if ( anim )
            anim.SetBool(Settings.instance.anim_reload, true);
        
        if ( audioSource ){
            audioSource.clip = reloadSound;
            audioSource.Play();
        }
    }
    // Performs reloading logic
    public void Reload(){
        int desiredAmmoUsed = maxClipSize - clipSize;
        if ( desiredAmmoUsed <= carryingAmmo ){
            carryingAmmo -= desiredAmmoUsed;
            clipSize = maxClipSize;
        } else {
            clipSize = carryingAmmo;
            carryingAmmo = 0;
        }
    }

    // Perform bullet logic
    private void Fire(){
        if ( !bulletSpawn ) return;

        // Sound
        if ( audioSource ){
            audioSource.clip = fireSound;
            audioSource.Play();
        }

        // Recoil
        Vector3 direction = bulletSpawn.forward + (Vector3)Random.insideUnitCircle*(aiming ? bulletSpread*0.5f : bulletSpread);

        RaycastHit hit;
        if ( Physics.Raycast(bulletSpawn.position, direction, out hit, atkRange) ){
            if ( debug ) Debug.DrawRay(bulletSpawn.position, hit.point-bulletSpawn.position, Color.red, 1f);

            // Ammo
            if ( clipSize > 0 ){
                clipSize--;
            } else {
                Aim(false);

                audioSource.clip = reloadSound;
                audioSource.Play();
                anim.SetBool(Settings.instance.anim_reload, true);
                return;
            }

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
        // Reverse aiming
        aiming = b;

        // Adjust transforms
        if ( aiming ){
            camTrans.SetParent(transform);
            camTrans.localPosition = aimPos;
        } else {
            camTrans.SetParent(headTrans);
            camTrans.localPosition = equipCamPos;
            camTrans.localEulerAngles = Vector3.zero;
        }
    }
}
