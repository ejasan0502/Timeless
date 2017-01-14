using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class SkillLibrary : MonoBehaviour {
    
    public List<Skill> skills { get; private set; }

    public void AddSkill(string id){
        
    }
    public int GetSkillIndex(Skill s){
        for (int i = 0; i < skills.Count; i++){
            if ( skills[i].id == s.id )
                return i;
        }
        return -1;
    }
}
