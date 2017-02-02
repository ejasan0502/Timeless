using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class BuffSkill : Skill {

    public bool percent;
    public CharStats charStats;
    public EquipStats equipStats;

    public BuffSkill() : base(){
        percent = false;
        charStats = new CharStats();
        equipStats = new EquipStats();
    }
    public BuffSkill(BuffSkill s) : base(s){
        percent = s.percent;
        charStats = new CharStats(s.charStats);
        equipStats = new EquipStats(s.equipStats);
    }

    public override object Self {
        get{
            return this;
        }
    }
    public override void Apply(Character caster, List<Character> targets){
        foreach (Character c in targets){
            c.AddBuff(this);
        }
    }
}
