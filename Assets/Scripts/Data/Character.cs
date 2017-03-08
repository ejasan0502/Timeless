using UnityEngine;
using System.Collections;

// Handles character object logic
public class Character : MonoBehaviour {

    public CharStats currentCharStats;
    public CharStats maxCharStats;

    void Start(){
        currentCharStats = new CharStats(maxCharStats);
    }

}
