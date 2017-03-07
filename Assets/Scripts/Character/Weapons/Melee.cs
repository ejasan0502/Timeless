using UnityEngine;
using System.Collections;

public class Melee : Weapon {

    [Header("-Audio-")]
    public AudioClip atkSound;

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

    // Perform attacking logic
    private void Attack(){
        atkCounter++;
        if ( atkCounter > 3 ){
            atkCounter = 1;
        }

        anim.SetInteger(Settings.instance.anim_attack, atkCounter);

        audioSource.clip = atkSound;
        audioSource.Play();
    }
}
