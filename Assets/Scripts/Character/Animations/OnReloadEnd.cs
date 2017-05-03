using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// When reloading animation is done, perform reload logic
public class OnReloadEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        base.OnStateExit(animator, stateInfo, layerIndex);

        animator.SetBool(Settings.instance.anim_reload, false);

        List<Weapon> weapons = animator.GetComponent<WeaponHandler>().currentWeapons;
        if ( weapons.Count > 0 ){
            IEnumerable<Weapon> firearms = weapons.Where((weapon) => weapon.isFirearm);

            foreach (Weapon weapon in firearms){
                Firearm firearm = weapon as Firearm;
                firearm.Reload();
            }
        }
    }

}
