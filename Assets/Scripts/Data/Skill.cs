using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class Skill {

    public string name;
    public string id;
    public string description;
    public SkillType skillType;
    public ElementType elementType;
    public string iconPath;
    public TargetType targetType;
    public CastAnim castAnim;
    public float castTime;
    public float range;

    public float hpCost;
    public float mpCost;
    public float staCost;

    public Sprite Icon {
        get {
            return Resources.Load<Sprite>(iconPath) ?? null;
        }
    }
    public virtual object Self {
        get {
            return this;
        }
    }

    public Skill(){
        name = "Untitled";
        id = "";
        description = "No Description.";
        skillType = SkillType.damage;
        elementType = ElementType.physical;
        iconPath = "Icons/default";
        targetType = TargetType.singleTarget;
        castAnim = CastAnim.instantMelee;
        castTime = 0f;
        range = 0f;

        hpCost = 0f;
        mpCost = 0f;
        staCost = 0f;
    }
    public Skill(Skill s){
        name = s.name;
        id = s.id;
        description = s.name;
        skillType = s.skillType;
        elementType = s.elementType;
        iconPath = s.iconPath;
        targetType = s.targetType;
        castAnim = s.castAnim;
        castTime = s.castTime;
        range = s.range;

        hpCost = s.hpCost;
        mpCost = s.mpCost;
        staCost = s.staCost;
    }
    
    protected bool CanCast(Character caster){
        return caster.currentStats.hp > hpCost && caster.currentStats.mp >= mpCost && caster.currentStats.sta >= staCost;
    }
    protected void Consume(Character caster){
        caster.currentStats.hp -= hpCost;
        caster.currentStats.mp -= mpCost;
        caster.currentStats.sta -= staCost;
    }
    public List<Character> FindTargets(Character caster){
        List<Character> targets = new List<Character>();
        GameObject o = null;

        switch (targetType){
        case TargetType.aoePoint:
        break;
        case TargetType.frontAoe:
        o = (GameObject) GameObject.Instantiate(Resources.Load("aoeSphere"));

        float size = range*0.5f;
        o.transform.localScale = new Vector3(size,size,size);
        o.transform.position = caster.transform.forward*range;
        break;
        case TargetType.frontAoeRect:
        o = (GameObject) GameObject.Instantiate(Resources.Load("aoeCube"));
        o.transform.position = caster.transform.position;
        o.transform.rotation = caster.transform.rotation;

        BoxCollider bc = o.GetComponent<BoxCollider>();
        bc.size = new Vector3(0.25f*range,0.25f*range,range);
        bc.center = new Vector3(0f,0f,range/2.0f);
        break;
        case TargetType.self:
        targets.Add(caster);
        break;
        case TargetType.singleTarget:
        if ( caster.Target != null ){
            targets.Add(caster.Target);
        }
        break;
        case TargetType.withinRadius:
        o = (GameObject) GameObject.Instantiate(Resources.Load("aoeSphere"));
        o.transform.position = caster.transform.position;
        o.transform.localScale = new Vector3(range,range,range);
        break;
        }

        return targets;
    }

    public virtual void Cast(Character caster){
        if ( !CanCast(caster) ) return;

        int skillIndex = caster.GetComponent<SkillLibrary>().GetSkillIndex(this);
        if ( targetType == TargetType.aoePoint ){
            EventManager.instance.TriggerEvent("OnSelectAoePoint", new MyEventArgs(new ArrayList(){skillIndex}));
        } else {
            if ( targetType == TargetType.singleTarget && caster.Target == null ) return;

            caster.View.SendReliable("CastInput", RpcTarget.Server, skillIndex);
        }
    }
    public virtual void Apply(Character caster, List<Character> targets){}
}
