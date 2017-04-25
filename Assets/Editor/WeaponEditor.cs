using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Weapon),true)]
public class WeaponEditor : Editor {

    private Weapon weapon;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        weapon = (Weapon) target;
        
        if ( GUILayout.Button("Clear All Positions") ){
            weapon.Clear();
        }

        if ( GUILayout.Button("Save spine rotation offset") ){
            if ( weapon.CharModel ){
                weapon.spineRotOffset = weapon.CharModel.spine1.localEulerAngles;
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
    }

}
