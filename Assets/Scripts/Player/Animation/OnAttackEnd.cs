using UnityEngine;
using System.Collections;

public class OnAttackEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetBool("attack",false);
    }

}
