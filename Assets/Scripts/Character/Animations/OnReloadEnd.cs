using UnityEngine;
using System.Collections;

// When reloading animation is done, perform reload logic
public class OnReloadEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        base.OnStateExit(animator, stateInfo, layerIndex);

        animator.SetBool(Settings.instance.anim_reload, false);

        Firearm weapon = (Firearm) animator.GetComponent<WeaponHandler>().currentWeapon;
        if ( weapon != null )
            weapon.Reload();
    }

}
