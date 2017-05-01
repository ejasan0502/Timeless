using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles generic weapon logic
[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(AudioSource))]
public class Weapon : MonoBehaviour {

    [Header("-Weapon Info-")]
    public EquipType weaponType;
    public float atkRate;
    public float atkRange;

    [Header("-Equip Settings-")]
    public Vector3 camOffset;
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
    protected CharacterModel charModel;

    public CharacterModel CharModel {
        get {
            return charModel;
        }
    }

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
    // Check if weapon is a tool
    public virtual bool isTool {
        get {
            return false;
        }
    }

    // Set animator
    public void SetAnim(Animator anim){
        this.anim = anim;
        character = anim.GetComponent<Character>();
        charModel = anim.GetComponent<WeaponHandler>().charModel;
    }
    // Clear all positions
    public virtual void Clear(){
        camOffset = Vector3.zero;
        equipPos = Vector3.zero;
        equipRot = Vector3.zero;
        unequipPos = Vector3.zero;
        unequipRot = Vector3.zero;
    }

    // Equip current weapon object to hand transform
    public void Equip(Transform hand){
        Drop(false);

        gameObject.SetActive(true);
        transform.SetParent(hand);
        transform.localPosition = equipPos;
        transform.localEulerAngles = equipRot;
    }
    // Unequip current weapon object to holster transform
    public void Unequip(CharacterModel charModel){
        if ( holster != HolsterType.none ){
            Transform holsterTrans = null;
            if ( holster == HolsterType.left ) holsterTrans = charModel.leftHolster;
            else if ( holster == HolsterType.right ) holsterTrans = charModel.rightHolster;
            else if ( holster == HolsterType.back ) holsterTrans = charModel.backHolster;

            transform.SetParent(holsterTrans);
            transform.localPosition = unequipPos;
            transform.localEulerAngles = unequipRot;
        } else {
            transform.SetParent(charModel.transform);
            gameObject.SetActive(false);
        }

        if ( !isMelee )
            col.enabled = false;
    }
    // Drop weapon object
    public virtual void Drop(bool dropItem){
        rb.isKinematic = !dropItem;
        rb.useGravity = dropItem;
        col.enabled = dropItem;
    }
    // Play a sound from weapon
    public void PlaySound(AudioClip sound){
        audioSource.clip = sound;
        audioSource.Play();
    }
    // Play a sound from weapon with a delay
    public void PlayDelayedSound(AudioClip sound, float delay){
        audioSource.clip = sound;
        audioSource.PlayDelayed(delay);
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
