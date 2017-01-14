using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public CharStats baseCharStats;
    public EquipStats baseEquipStats;

    private SkillLibrary skillLib;
    private Character character;

    void Awake(){
        skillLib = GetComponent<SkillLibrary>();
        character = GetComponent<Character>();
    }

    public void UpdateStats(){
        character.maxStats = baseCharStats;
        character.maxEquipStats = baseEquipStats;

        skillLib.ApplyPassives(character);
    }
}
