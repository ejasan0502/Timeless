using UnityEngine;
using System.Reflection;
using System.Collections;

// Save stats relating to attacking or casting
[System.Serializable]
public class CombatStats {
    
    public float atkRange;
    public float atkRate;
    
    public CombatStats(){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, 0f);
        }
    }
    public CombatStats(float val){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, val);
        }
    }
    public CombatStats(CombatStats val){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] fields2 = val.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, fields2[i].GetValue(val));
        }
    }

}
