using UnityEngine;
using System.Reflection;
using System.Collections;

// Save stats relating to attacking or casting
[System.Serializable]
public class CombatStats {
    
    public float minMeleeDmg;
    public float maxMeleeDmg;
    public float minMagicDmg;
    public float maxMagicDmg;
    public float minRangeDmg;
    public float maxRangeDmg;

    public float physDef;
    public float magicDef;

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

    public static CombatStats operator+(CombatStats stats1, CombatStats stats2){
        CombatStats stats = new CombatStats();
        FieldInfo[] fields = stats.GetType().GetFields();
        FieldInfo[] fields1 = stats1.GetType().GetFields();
        FieldInfo[] fields2 = stats2.GetType().GetFields();
        
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(stats, (float)fields1[i].GetValue(stats1) + (float)fields2[i].GetValue(stats2));
        }

        return stats;
    }
    public static CombatStats operator-(CombatStats stats1, CombatStats stats2){
        CombatStats stats = new CombatStats();
        FieldInfo[] fields = stats.GetType().GetFields();
        FieldInfo[] fields1 = stats1.GetType().GetFields();
        FieldInfo[] fields2 = stats2.GetType().GetFields();
        
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(stats, (float)fields1[i].GetValue(stats1) - (float)fields2[i].GetValue(stats2));
        }

        return stats;
    }
    public static CombatStats operator*(CombatStats stats1, CombatStats stats2){
        CombatStats stats = new CombatStats();
        FieldInfo[] fields = stats.GetType().GetFields();
        FieldInfo[] fields1 = stats1.GetType().GetFields();
        FieldInfo[] fields2 = stats2.GetType().GetFields();
        
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(stats, (float)fields1[i].GetValue(stats1) * (float)fields2[i].GetValue(stats2));
        }

        return stats;
    }
}
