using UnityEngine;
using System.Collections;

// Perform when tool attack animation ends
public class OnToolEnd : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetInteger(Settings.instance.anim_attack, 0);

        Character character = animator.GetComponent<Character>();
        if ( character && character.IsPlayer ){
            Inventory inventory = character.GetComponent<Inventory>();

            RaycastHit hit;
            Vector3 direction = Camera.main.transform.forward;
            if ( Physics.Raycast(new Ray(Camera.main.transform.position, direction), out hit, 2f, 1 << LayerMask.NameToLayer("Resource")) ){
                Resource resource = hit.collider.GetComponent<Resource>();
                int amt = resource.Hit(character.MeleeDamage);

                inventory.AddItem(resource.resourceId, amt);
            }
        }
    }

}
