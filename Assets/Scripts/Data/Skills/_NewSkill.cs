using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class _NewSkill : Skill {

    public _NewSkill() : base(){

    }
    public _NewSkill(_NewSkill s) : base(s){

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
