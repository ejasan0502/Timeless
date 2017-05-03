using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Saves list of enemy characters within a radius around this character
[RequireComponent(typeof(SphereCollider))]
public class AIRadius : MonoBehaviour {

    public List<Character> enemiesWithinRange = new List<Character>();

    void Awake(){
        enemiesWithinRange = new List<Character>();
    }
    void OnTriggerEnter(Collider other){
        Character c = other.GetComponent<Character>();
        if ( c != null && c.IsAlive && c.tag != transform.parent.tag && !enemiesWithinRange.Contains(c) ){
            enemiesWithinRange.Add(c);
        }
        c = null;
    }
    void OnTriggerExit(Collider other){
        Character c = other.GetComponent<Character>();
        if ( c != null && enemiesWithinRange.Contains(c) ){
            enemiesWithinRange.Add(c);
        }
    }
}