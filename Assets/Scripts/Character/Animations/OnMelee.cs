using UnityEngine;
using System.Collections;

public class OnMelee : StateMachineBehaviour {

    private Monster m = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if ( animator.GetComponent<Monster>() != null ){
            m = animator.GetComponent<Monster>();
            m.PlaySound(m.atkSounds[animator.GetInteger(Settings.instance.anim_attack)-1]);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetInteger(Settings.instance.anim_attack, 0);
        if ( m != null ){
            m.GetComponent<AudioSource>().Stop();
        }
    }

}
