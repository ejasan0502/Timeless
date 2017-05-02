using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Perform when tool attack animation ends
public class OnToolEnd : StateMachineBehaviour {

    public float soundDelay = 1f;
    private Character c = null;
    private Tool weapon;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        c = animator.GetComponent<Character>();
        if ( c != null ){
            weapon = c.GetComponent<WeaponHandler>().currentWeapon as Tool;
            weapon.PlayDelayedSound(weapon.atkSound,soundDelay);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if ( c != null && weapon != null ){
            Inventory inventory = c.GetComponent<Inventory>();
            foreach (Resource target in weapon.targets){
                int amt = target.Hit(c.MeleeDamage);
                inventory.AddItem(target.resourceId, amt);
            }
            weapon.targets = new List<Resource>();
        }
    }

}
