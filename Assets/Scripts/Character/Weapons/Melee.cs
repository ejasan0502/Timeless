using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles melee weapon logic
public class Melee : Weapon {

    public float staminaCost;
    public AudioClip atkSound;

    public List<Character> targets = new List<Character>();

    private int atkCounter = 0;

    // Override bool to true since weapon is melee
    public override bool isMelee
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
    // Blocking
    public override void SecondaryFire(){
        anim.SetInteger(Settings.instance.anim_attack, -1);
    }
    // Stop blocking
    public override void SecondaryFireEnd(){
        anim.SetInteger(Settings.instance.anim_attack, 0);
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

        atkCounter++;
        if ( atkCounter > 3 ){
            atkCounter = 1;
        }

        anim.SetInteger(Settings.instance.anim_attack, atkCounter);

        //audioSource.clip = atkSound;
        //audioSource.Play();
    }

    void OnTriggerEnter(Collider other){
        Character c = other.GetComponent<Character>();
        if ( c != null && c != character && !targets.Contains(c) ){
            targets.Add(c);
        }
    }
}
