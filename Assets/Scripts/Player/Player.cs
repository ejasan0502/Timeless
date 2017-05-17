using UnityEngine;
using System.Collections;

public class Player : Character {

    protected override void Awake(){
        base.Awake();

        maxCharStats = new CharStats(Settings.instance.base_player_charStats);
        maxCombatStats = new CombatStats();
        currentCharStats = new CharStats(maxCharStats);
        currentCombatStats = new CombatStats(maxCombatStats);
    }

}
