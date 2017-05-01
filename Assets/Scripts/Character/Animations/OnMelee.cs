using UnityEngine;
using System.Collections;

// Handle melee attacks
public class OnMelee : StateMachineBehaviour {

    public float soundDelay = 1f;
    private Character c = null;
    private Melee weapon;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        c = animator.GetComponent<Character>();
        if ( c != null ){
            weapon = c.GetComponent<WeaponHandler>().currentWeapon as Melee;
            weapon.PlayDelayedSound(weapon.atkSound,soundDelay);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if ( c != null && weapon != null ){
            foreach (Character target in weapon.targets){
                target.Hit(c, c.MeleeDamage, InflictType.melee);
            }
        }
    }

}
