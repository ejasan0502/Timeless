using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tool : Weapon {

    public float staminaCost;
    public AudioClip atkSound;

    public List<Resource> targets = new List<Resource>();

    private int atkCounter = 0;

    // Override bool to true since weapon is melee
    public override bool isTool
    {
        get
        {
            return true;
        }
    }

    // Normal attack
    public override void SinglePrimaryFire(){
        Attack();
    }

    // Override Dropping
    public override void Drop(bool dropItem){
        rb.isKinematic = !dropItem;
        rb.useGravity = dropItem;
        col.isTrigger = !dropItem;
    }

    // Perform attacking logic
    private void Attack(){
        if ( character ){
            if ( character.currentCharStats.stamina >= staminaCost ){
                character.currentCharStats.stamina -= staminaCost;
            } else {
                return;
            }
        }

        anim.SetInteger(Settings.instance.anim_attack, -1);

        //audioSource.clip = atkSound;
        //audioSource.Play();
    }

    void OnTriggerEnter(Collider other){
        Resource c = other.GetComponent<Resource>();
        if ( c != null && !targets.Contains(c) ){
            targets.Add(c);
        }
    }
}
