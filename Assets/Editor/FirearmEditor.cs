﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Firearm))]
public class FirearmEditor : Editor {

    private Firearm weapon;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        
        weapon = (Firearm) target;
        
        if ( GUILayout.Button("Clear All Positions") ){
            weapon.Clear();
        }

        if ( GUILayout.Button("Save charModel offset position and rotation") ){
            if ( weapon.CharModel ){
                weapon.camPosOffset = weapon.CharModel.transform.localPosition;
                weapon.camRotOffset = weapon.CharModel.transform.localEulerAngles;
            }
        }
        
        if ( GUILayout.Button("Save equip position and rotation") ){
            weapon.equipPos = weapon.transform.localPosition;
            weapon.equipRot = weapon.transform.localEulerAngles;
        }

        if ( GUILayout.Button("Save unequip position and rotation") ){
            weapon.unequipPos = weapon.transform.localPosition;
            weapon.unequipRot = weapon.transform.localEulerAngles;
        }

        if ( GUILayout.Button("Save aim position and rotation") ){
            if ( weapon.CharModel ){
                weapon.aimPos = weapon.CharModel.transform.localPosition;
                weapon.aimRot = weapon.CharModel.transform.localEulerAngles;
            }
        }
    }

}
