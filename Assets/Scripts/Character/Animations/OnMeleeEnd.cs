using UnityEngine;
using System.Collections;

public class OnMeleeEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetInteger(Settings.instance.anim_attack, 0);
    }

}
