using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveSkill : Skill {

    public bool percent;
    public CharStats charStats;
    public EquipStats equipStats;

    public PassiveSkill() : base(){
        percent = false;
        charStats = new CharStats(0f);
        equipStats = new EquipStats(0f);
    }
    public PassiveSkill(PassiveSkill s) : base(s){
        percent = s.percent;
        charStats = new CharStats(s.charStats);
        equipStats = new EquipStats(s.equipStats);
    }

    public override object Self {
        get{
            return this;
        }
    }
    public override void Cast(Character caster){
        base.Cast(caster);
    }
    public override void Apply(Character caster, List<Character> targets){
        base.Apply(caster,targets);
    }
}
