using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public CharStats baseCharStats;
    public EquipStats baseEquipStats;

    private Character character;

    void Awake(){
        character = GetComponent<Character>();
    }

    public void UpdateStats(){
        character.maxStats = baseCharStats;
        character.maxEquipStats = baseEquipStats;
    }
}
