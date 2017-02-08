using UnityEngine;
using System.Collections;
using MassiveNet;

public class OnAttackEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetInteger("attack", 0);

        //Character atker = animator.transform.parent.GetComponent<Character>();
        //if ( atker != null ){
        //    atker.BasicAttack();
        //    if ( atker.View.IsController(Client.instance.Socket.Self) ){
        //        atker.View.SendReliable("BasicAttack", RpcTarget.Server);
        //    }
        //}
    }

}
