using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Weapon),true)]
public class WeaponEditor : Editor {

    private Weapon weapon;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        weapon = (Weapon) target;
        
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
    }

}
