using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    private Dictionary<string,List<Skill>> skills = new Dictionary<string,List<Skill>>();

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
