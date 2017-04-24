using UnityEngine;
using System.Collections;

public class Player : Character {

    protected override void Awake(){
        base.Awake();

        maxCharStats = new CharStats(Settings.instance.base_player_charStats);
        maxCombatStats = new CombatStats(Settings.instance.base_player_combatStats);
        currentCharStats = new CharStats(maxCharStats);
        currentCombatStats = new CombatStats(maxCombatStats);
    }

}
