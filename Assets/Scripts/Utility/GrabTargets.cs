using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabTargets : MonoBehaviour {

    private List<Character> targets;

    void Awake(){
        targets = new List<Character>();
    }
    void OnTriggerEnter(Collider other){
        Character c = other.GetComponent<Character>();
        if ( c != null ){
            targets.Add(c);
        }
    }
    void OnTriggerExit(Collider other){
        Character c = other.GetComponent<Character>();
        if ( c != null ){
            targets.Remove(c);
        }   
    }

    public string[] GetTargets(string targetTag){
        List<string> desiredTargets = new List<string>();
        for (int i = targets.Count-1; i >= 0; i--){
            if ( targets[i] == null ){
                targets.RemoveAt(i);
            } else if ( !targets[i].IsAlive ){
                targets.RemoveAt(i);
            } else {
                desiredTargets.Add(targets[i].id);
            }
        }

        return desiredTargets.ToArray();
    }
}
