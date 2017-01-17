using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class BuffSkill : Skill {

    public bool percent;
    public CharStats charStats;
    public EquipStats equipStats;

    public BuffSkill() : base(){

    }
    public BuffSkill(BuffSkill s) : base(s){

    }

    public override object Self {
        get{
            return this;
        }
    }
    public override void Cast(Character caster){
        if ( !CanCast(caster) ) return;

        int skillIndex = caster.GetComponent<SkillLibrary>().GetSkillIndex(this);
        if ( targetType == TargetType.aoePoint ){
            EventManager.instance.TriggerEvent("OnSelectAoePoint", new MyEventArgs(new ArrayList(){skillIndex}));
        } else {
            if ( targetType == TargetType.singleTarget && caster.Target == null ) return;

            caster.View.SendReliable("CastInput", RpcTarget.Server, skillIndex);
        }
    }
    public override void Apply(Character caster, List<Character> targets){
        foreach (Character c in targets){
            
        }
    }
}
