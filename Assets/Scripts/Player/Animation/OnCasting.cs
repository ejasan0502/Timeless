using UnityEngine;
using System.Collections;

public class OnCasting : StateMachineBehaviour {

    private Character caster = null;
    private float startCastTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
       caster = animator.transform.parent.GetComponent<Character>();
       startCastTime = Time.time;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if ( caster != null && Time.time - startCastTime > caster.CastSkill.castTime ){
            caster.ApplyCast();
        }
    }

}
