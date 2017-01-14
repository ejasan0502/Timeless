using UnityEngine;
using System.Collections;

public class SkillHotkey : Hotkey {

    public Skill skill;

    public SkillHotkey() : base(){
        skill = null;
    }

    public override void Apply(){
        if ( skill != null ){
            skill.Cast(PlayerOwner.instance.character);
        }
    }
}
