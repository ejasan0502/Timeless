using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class StatusSkill : Skill {

    public List<Status> statuses;
    public List<float> durations;
    public List<float> amounts;

    public StatusSkill() : base(){
        statuses = new List<Status>();
        durations = new List<float>();
        amounts = new List<float>();
    }
    public StatusSkill(StatusSkill s) : base(s){
        statuses = s.statuses;
        durations = s.durations;
        amounts = s.amounts;
    }

    public override object Self {
        get{
            return this;
        }
    }
    public override void Apply(Character caster, List<Character> targets){
        foreach (Character c in targets){
            for (int i = 0; i < statuses.Count; i++){
                
            }
        }
    }
}
