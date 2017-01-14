using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class SkillLibrary : MonoBehaviour {
    
    public List<Skill> skills { get; private set; }

    void Awake(){
        skills = new List<Skill>();
    }

    public bool HasSkill(string id){
        return skills.Where(s => s.id == id).FirstOrDefault<Skill>() != null;
    }
    public void AddSkill(string id){
        if ( HasSkill(id) ) return;

        Skill skill = SkillManager.GetSkill(id);
        if ( skill != null ){
            skills.Add(skill);
        }
    }
    public int GetSkillIndex(Skill s){
        for (int i = 0; i < skills.Count; i++){
            if ( skills[i].id == s.id )
                return i;
        }
        return -1;
    }
    public void ApplyPassives(Character caster){
        List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
        foreach (Skill s in skills){
            if ( s.skillType == SkillType.passive )
                passiveSkills.Add((PassiveSkill)s.Self);
        }

        CharStats charStats = new CharStats(0f);
        EquipStats equipStats = new EquipStats(0f);
        CharStats charStatsP = new CharStats(1f);
        EquipStats equipStatsP = new EquipStats(1f);
        foreach (PassiveSkill s in passiveSkills){
            if ( s.percent ){
                charStatsP += s.charStats;
                equipStatsP += s.equipStats;
            } else {
                charStats += s.charStats;
                equipStats += s.equipStats;
            }
        }

        caster.maxStats += charStats;
        caster.maxEquipStats += equipStats;
        caster.maxStats *= charStatsP;
        caster.maxEquipStats *= equipStatsP;
    }
}
