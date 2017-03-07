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
    // Blocking
    public override void SecondaryFire(){
        anim.SetInteger(Settings.instance.anim_attack, -1);
    }
    // Stop blocking
    public override void SecondaryFireEnd(){
        anim.SetInteger(Settings.instance.anim_attack, 0);
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
