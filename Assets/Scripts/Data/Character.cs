using UnityEngine;
using System.Collections;

// Handles character object logic
public class Character : MonoBehaviour {

    [Header("-Character Info-")]
    public CharStats currentCharStats;
    public CombatStats currentCombatStats;

    public CharStats maxCharStats;
    public CombatStats maxCombatStats;

    public bool IsAlive {
        get {
            return currentCharStats.health > 0;
        }
    }

    protected virtual void Awake(){
        currentCharStats = new CharStats(maxCharStats);
        currentCombatStats = new CombatStats(maxCombatStats);

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
