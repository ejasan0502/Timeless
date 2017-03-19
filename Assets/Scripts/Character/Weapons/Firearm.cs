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
    public GameObject bloodRef;
    public GameObject decalRef;
    public GameObject muzzleFlashRef;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [Header("-Positions-")]
    public Transform bulletSpawn;
    public Vector3 aimPos;
    public Vector3 aimRot;

    private bool aiming = false;
    private bool canFire = true;

    // Override bool to true since weapon is firearm
    public override bool isFirearm {
        get {
            return true;
        }
    }

    // Single fire on button down
    public override void SinglePrimaryFire(){
        if ( autoFire || anim.GetBool(Settings.instance.anim_reload) ) return;

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
        if ( !bulletSpawn || !canFire ) return;

        // Sound
        if ( audioSource ){
            audioSource.clip = fireSound;
            audioSource.Play();
        }

        // Recoil
        Vector3 direction = bulletSpawn.forward + (Vector3)Random.insideUnitCircle*(aiming ? bulletSpread*0.1f : bulletSpread);

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
        
        // Muzzle Flash
        if ( muzzleFlashRef ){
            GameObject muzzleFlash = Instantiate(muzzleFlashRef, bulletSpawn.position, Quaternion.identity, bulletSpawn) as GameObject;
            Destroy(muzzleFlash, 0.2f);
        }

        RaycastHit hit;
        if ( Physics.Raycast(bulletSpawn.position, direction, out hit, atkRange) ){
            if ( debug ) Debug.DrawRay(bulletSpawn.position, hit.point-bulletSpawn.position, Color.red, 1f);

            // Decal
            GameObject decal = null;
            if ( hit.collider.gameObject.isStatic ){
                if ( decalRef ){
                    decal = Instantiate(decalRef, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                }
            } else {
                if ( bloodRef ){
                    decal = Instantiate(bloodRef, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                }

                Character c = hit.collider.GetComponent<Character>();
                if ( c != null ){
                    c.Hit(character, character.RangeDamage, InflictType.range);
                }
            }
            Debug.Log(hit.collider.transform.name);
            decal.transform.SetParent(hit.collider.transform);
            Destroy(decal, 2f);
        }

        // Fire delay
        canFire = false;
        StartCoroutine(FireDelay());
    }
    // Perform aiming logic
    private void Aim(bool b){
        aiming = b;

        if ( aiming ){
            Camera.main.fieldOfView = Settings.instance.defaultFov/2.00f;
            charModel.transform.localPosition = aimPos;
            charModel.transform.localEulerAngles = aimRot;
        } else {
            Camera.main.fieldOfView = Settings.instance.defaultFov;
            charModel.transform.localPosition = camPosOffset;
            charModel.transform.localEulerAngles = camRotOffset;
        }
    }
    // Apply fire delay
    private IEnumerator FireDelay(){
        yield return new WaitForSeconds(atkRate);
        canFire = true;
    }
}
