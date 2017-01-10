using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    private Dictionary<string,List<Skill>> skills = new Dictionary<string,List<Skill>>(){
        { "1hw", new List<Skill>(){
            new DamageSkill("Heavy Swing", "Deals physical damage to a target.", new EquipStats(1f,3f), ElementType.physical, CastAnim.swing, 0f, 3f, "Icons/default", 1, 2f, AoeType.singleTarget, 0f, 0f, 0f, false)
            }
        }
    };

    private static SkillManager _instance;
    public static SkillManager instance {
        get {
            if ( _instance == null )
                _instance = GameObject.FindObjectOfType<SkillManager>();

            return _instance;
        }
    }

    public static Skill GetSkill(string id){
        string[] args = id.Split('-');
        if ( instance.skills.ContainsKey(args[0]) ){
            int index = 0;
            if ( int.TryParse(args[1], out index) ){
                if ( index < instance.skills[args[0]].Count ){
                    return instance.skills[args[0]][index];
                }
            }
        }

        return null;
    }
}
