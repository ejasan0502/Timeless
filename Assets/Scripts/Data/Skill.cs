using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public abstract class Skill {

    public string name;
    public string description;
    public EquipStats stats;
    public ElementType elementType;
    public CastAnim castAnim;
    public float castTime;              // -1 = toggle, 0 = instant, 1+ = duration
    public float cooldown;
    public string iconPath;

    public int maxTargets;
    public float aoeRadius;
    public AoeType aoeType;

    public float hpCost;
    public float mpCost;
    public float staCost;

    public bool friendly;

    protected float startCastTime;

    public Sprite Icon {
        get {
            return Resources.Load<Sprite>(iconPath) ?? null;
        }
    }
    public bool CanCast(Character c){
        return c.currentStats.hp > hpCost && c.currentStats.mp >= mpCost && c.currentStats.sta > staCost;
    }
    public bool InCooldown {
        get {
            return Time.time - startCastTime < cooldown;
        }
    }
    public bool IsToggle {
        get {
            return castTime == -1;
        }
    }
    public bool IsInstant {
        get {
            return castTime == 0;
        }
    }

    public Skill(){
        name = "Untitled";
        description = "No Description.";
        stats = new EquipStats();
        elementType = ElementType.physical;
        castAnim = CastAnim.swing;
        castTime = 1f;
        cooldown = 1f;
        iconPath = "Icons/default";

        maxTargets = 1;
        aoeRadius = 1f;
        aoeType = AoeType.singleTarget;

        hpCost = 0f;
        mpCost = 0f;
        staCost = 0f;

        friendly = false;
    }
    public Skill(Skill s){
        name = s.name;
        description = s.name;
        stats = new EquipStats(stats);
        elementType = s.elementType;
        castAnim = s.castAnim;
        castTime = s.castTime;
        cooldown = s.cooldown;
        iconPath = s.iconPath;

        maxTargets = s.maxTargets;
        aoeRadius = s.aoeRadius;
        aoeType = s.aoeType;

        hpCost = s.hpCost;
        mpCost = s.mpCost;
        staCost = s.staCost;

        friendly = s.friendly;
    }

    public void Consume(Character caster){
        caster.currentStats.hp -= hpCost;
        caster.currentStats.mp -= mpCost;
        caster.currentStats.sta -= staCost;
    }

    public abstract void Cast(Character caster);
    public abstract void Apply(List<Character> targets);
}
