using UnityEngine;
using System.Collections;

public class ActionControl : MonoBehaviour {
	
    private const int maxAtkCounter = 4;

    private Animator anim;
    private Character c;
    private Equipment equipment;
    private int atkCounter = 0;

    void Awake(){
        c = GetComponent<Character>();
        anim = GetComponent<Animator>();
        equipment = GetComponent<Equipment>();
    }
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
