using UnityEngine;
using System.Collections;

public class ActionControl : MonoBehaviour {
	
    private const int maxAtkCounter = 4;

    public Animator anim;
    private int atkCounter = 0;

    void Update(){
        if ( Input.GetMouseButtonDown(0) ){
            if ( !anim.GetBool("combat") ) anim.SetBool("combat",true);

            atkCounter++;
            if ( atkCounter > maxAtkCounter ){
                atkCounter = 1;
            }
            anim.SetInteger("attack",atkCounter);
        }
    }

}
