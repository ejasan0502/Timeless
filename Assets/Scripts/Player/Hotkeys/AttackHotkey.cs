using UnityEngine;
using System.Collections;

public class AttackHotkey : Hotkey {

    public AttackHotkey(){
        key = KeyCode.Alpha1;
        iconPath = "Icons/Other/attack";
    }
    public override void Apply(){
        Character c = Character.main;
        c.Chase();
    }

}
