using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class DamageSkill : Skill {

    public bool percent;
    public float minDmg;
    public float maxDmg;

    public DamageSkill() : base(){
        percent = false;
        minDmg = 0f;
        maxDmg = 0f;
    }
    public DamageSkill(DamageSkill s) : base(s){
        percent = s.percent;
        minDmg = s.minDmg;
        maxDmg = s.maxDmg;
    }

    public override object Self {
        get{
            return this;
        }
    }
    public override void Apply(Character caster, List<Character> targets){
        float min, max;
        if ( elementType == ElementType.physical ){
            min = percent ? caster.currentEquipStats.minPhysDmg*minDmg : caster.currentEquipStats.minPhysDmg+minDmg;
            max = percent ? caster.currentEquipStats.maxPhysDmg*maxDmg : caster.currentEquipStats.maxPhysDmg+maxDmg;
        } else if ( elementType == ElementType.ranged ){
            min = percent ? caster.currentEquipStats.minRangeDmg*minDmg : caster.currentEquipStats.minRangeDmg+minDmg;
            max = percent ? caster.currentEquipStats.maxRangeDmg*maxDmg : caster.currentEquipStats.maxRangeDmg+maxDmg;
        } else {
            min = percent ? caster.currentEquipStats.minMagDmg*minDmg : caster.currentEquipStats.minMagDmg+minDmg;
            max = percent ? caster.currentEquipStats.maxMagDmg*maxDmg : caster.currentEquipStats.maxMagDmg+maxDmg;
        }

        float rawDmg = Random.Range(min,max);

        foreach (Character c in targets){
            c.Hit(rawDmg);
        }
    }
}
