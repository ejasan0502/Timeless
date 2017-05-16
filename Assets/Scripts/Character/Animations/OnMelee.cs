using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handle melee attacks
public class OnMelee : StateMachineBehaviour {

    public int weaponIndex = -1;
    public float soundDelay = 1f;
    private Character c = null;
    private Melee weapon;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        c = animator.GetComponent<Character>();

        if ( c != null ){
            if ( weaponIndex == -1 ){
                // Character is not dual wielding
                weapon = c.GetComponent<WeaponHandler>().currentWeapons[0] as Melee;
            } else {
                // Character is dual wielding
                weapon = c.GetComponent<WeaponHandler>().currentWeapons[weaponIndex] as Melee;
            }
            weapon.PlayDelayedSound(weapon.atkSound,soundDelay);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if ( c != null && weapon != null ){
            foreach (Character target in weapon.targets){
                target.Hit(c, c.MeleeDamage, InflictType.melee);
            }
            weapon.targets = new List<Character>();
        }
    }

}
