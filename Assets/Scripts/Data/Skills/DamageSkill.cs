using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DamageSkill : Skill {

    public DamageSkill() : base(){
        
    }
    public DamageSkill(DamageSkill s) : base(s){
        
    }
    public DamageSkill(params object[] args){
        try {
            name = args.Length > 0 ? (string)args[0] : "Untitled";
            description = args.Length > 1 ? (string)args[1] : "No Description.";
            stats = args.Length > 2 ? new EquipStats((EquipStats)args[2]) : new EquipStats();
            elementType = args.Length > 3 ? (ElementType)args[3] : ElementType.physical;
            castAnim = args.Length > 4 ? (CastAnim)args[4] : CastAnim.swing;
            castTime = args.Length > 5 ? (float)args[5] : 1f;
            cooldown = args.Length > 6 ? (float)args[6] : 1f;
            iconPath = args.Length > 7 ? (string)args[7] : "Icons/default";

            maxTargets = args.Length > 8 ? (int)args[8] : 1;
            aoeRadius = args.Length > 9 ? (float)args[9] : 1f;
            aoeType = args.Length > 10 ? (AoeType)args[10] : AoeType.singleTarget;

            hpCost = args.Length > 11 ? (float)args[11] : 0f;
            mpCost = args.Length > 12 ? (float)args[12] : 0f;
            staCost = args.Length > 13 ? (float)args[13] : 0f;

            friendly = args.Length > 14 ? (bool)args[14] : false;
        } catch (Exception e){
            Debug.LogError(e.ToString());
        }
    }

    public override void Cast(Character caster){
        if ( CanCast(caster) && !InCooldown ){
            switch (aoeType){
            #region Single Target
            case AoeType.singleTarget:
            if ( caster.HasTarget ){
                if ( IsInstant ){
                    Apply(new List<Character>(){caster.Target});
                } else if ( IsToggle ){
                    caster.StartCoroutine(Toggle(caster));
                } else {
                    caster.SetAnimState("castAnim", (int)castAnim);
                }
            } else {
                Debug.Log("Select a target");
                EventManager.instance.TriggerEvent("OnTargetSelect", new MyEventArgs(new ArrayList(){this}));
            }
            break;
            #endregion
            }
        } else {
            Debug.Log("Unable to cast skill.");
        }
    }
    public override void Apply(List<Character> targets){
        
    }

    private IEnumerator Toggle(Character caster){
        while ( CanCast(caster) ){
            Consume(caster);
            Apply(new List<Character>(){caster.Target});
            yield return new WaitForSeconds(3f);
        }
    }
}
