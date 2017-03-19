using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPhysics : MonoBehaviour {

    private Collider col;

    void Awake(){
        col = GetComponent<Collider>();
    }
    void OnCollisionEnter(Collision other){
        Physics.IgnoreCollision(other.collider, col);
    }

}
