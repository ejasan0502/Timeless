using UnityEngine;
using System.Collections;

// Handles generic weapon logic
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Weapon : MonoBehaviour {

    [Header("-Weapon Info-")]
    public bool debug;
    public WeaponType weaponType;
    public float atkRate;
    public float atkRange;

    [Header("-Equip Settings-")]
    public Vector3 equipCamPos;
    public Vector3 equipPos;
    public Vector3 equipRot;
    public Vector3 unequipPos;
    public Vector3 unequipRot;
    public HolsterType holster;
    
    protected Collider col;
    protected Rigidbody rb;

    protected Animator anim;
    protected AudioSource audioSource;
    protected Character character;

    void Awake(){
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    // Check if weapon is firearm, overrided on firearm script
    public virtual bool isFirearm {
        get {
            return false;
        }
    }
    // Check if weapon is melee, override on melee script
    public virtual bool isMelee {
        get {
            return false;
        }
    }

    // Set animator
    public void SetAnim(Animator anim){
        this.anim = anim;
        character = anim.GetComponent<Character>();
    }

    // Equip current weapon object to hand transform
    public void Equip(Transform hand){
        Drop(false);

        transform.SetParent(hand);
        transform.localPosition = equipPos;
        transform.localEulerAngles = equipRot;

        Camera.main.transform.localPosition = equipCamPos;
    }
    // Unequip current weapon object to holster transform
    public void Unequip(Transform holster){
        transform.SetParent(holster);
        transform.localPosition = unequipPos;
        transform.localEulerAngles = unequipRot;

        col.enabled = false;
    }
    // Drop weapon object
    public void Drop(bool dropItem){
        rb.isKinematic = !dropItem;
        rb.useGravity = dropItem;
        col.enabled = dropItem;
    }
    // Play a sound from weapon
    public void PlaySound(AudioClip sound){
        audioSource.clip = sound;
        audioSource.Play();
    }

    // Handle single primary fire logic (single left click)
    public virtual void SinglePrimaryFire(){

    }
    // Handle primary fire logic (left click)
    public virtual void PrimaryFire(){

    }
    // Handle secondary fire logic (right click)
    public virtual void SecondaryFire(){

    }
    // Handle secondary fire end logic (right click up)
    public virtual void SecondaryFireEnd(){

    }
    // Handle alt fire logic (R)
    public virtual void AltFire(){
        
    }

}
