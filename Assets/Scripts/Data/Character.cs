using UnityEngine;
using System.Collections;

// Handles character object logic
public class Character : MonoBehaviour {

    public CharStats currentCharStats;
    public CharStats maxCharStats;

    public bool IsAlive {
        get {
            return currentCharStats.health > 0;
        }
    }

    void Start(){
        currentCharStats = new CharStats(maxCharStats);

        StartCoroutine(RecoveryRate());
    }

    // Apply recovery on stats every 3 seconds
    private IEnumerator RecoveryRate(){
        while ( IsAlive ){
            yield return new WaitForSeconds(3f);
            if ( currentCharStats.health < maxCharStats.health ){
                currentCharStats.health += currentCharStats.hpRecov;
            }
            if ( currentCharStats.shields < maxCharStats.shields ){
                currentCharStats.shields += currentCharStats.shldRecov;
            }
            if ( currentCharStats.mana < maxCharStats.mana ){
                currentCharStats.mana += currentCharStats.mpRecov;
            }
            if ( currentCharStats.stamina < maxCharStats.stamina ){
                currentCharStats.stamina += currentCharStats.staRecov;
            }
        }
    }
}
