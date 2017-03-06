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
    public Vector3 equipPos;
    public Vector3 equipRot;
    public Vector3 unequipPos;
    public Vector3 unequipRot;

    private Collider col;
    private Rigidbody rb;

    protected Animator anim;

    void Awake(){
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetAnim(Animator anim){
        this.anim = anim;
    }

    // Equip current weapon object to hand transform
    public void Equip(Transform hand){
        Drop(false);

        transform.SetParent(hand);
        transform.localPosition = equipPos;
        transform.localEulerAngles = equipRot;
    }
    // Unequip current weapon object to holster transform
    public void Unequip(Transform holster){
        transform.SetParent(holster);
        transform.localPosition = unequipPos;
        transform.localEulerAngles = unequipRot;
    }
    // Drop weapon object
    public void Drop(bool dropItem){
        rb.isKinematic = !dropItem;
        rb.useGravity = dropItem;
        col.enabled = dropItem;
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

}
