using UnityEngine;
using System.Collections;

public class Tool : Weapon {

    public float staminaCost;
    public AudioClip atkSound;

    // Weapon is a tool
    public override bool isTool
    {
        get
        {
            return true;
        }
    }

    // Perform primary action
    public override void SinglePrimaryFire(){
        Attack();
    }

    // Perform attack
    private void Attack(){
        if ( character ){
            if ( character.currentCharStats.stamina >= staminaCost ){
                character.currentCharStats.stamina -= staminaCost;
            } else {
                return;
            }
        }

        anim.SetInteger(Settings.instance.anim_attack, -1);

        audioSource.clip = atkSound;
        audioSource.Play();
    }
}
