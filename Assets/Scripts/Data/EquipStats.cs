using UnityEngine;
using System.Reflection;
using System.Collections;

public class EquipStats {

    public float minPhysDmg, maxPhysDmg;
    public float minMagDmg, maxMagDmg;
    public float minRangeDmg, maxRangeDmg;

    public float physDef;
    public float magDef;

    public float critChance;                    // Percent
    public float critDmg;                       // Percent

    public EquipStats(){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, 0f);
        }
    }
    public EquipStats(float val){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, val);
        }
    }
    public EquipStats(params float[] args){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            if ( i < fields.Length ){
                fields[i].SetValue(this, args[i]);
            }
        }
    }
    public EquipStats(EquipStats es){
        FieldInfo[] fields1 = GetType().GetFields();
        FieldInfo[] fields2 = es.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields1[i].SetValue(this, (float)fields2[i].GetValue(es));
        }
    }
    public EquipStats(string s){
        string[] args = s.Split('/');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            fields[i].SetValue(this, float.Parse(args[i]));
        }
    }

    public override string ToString(){
        string s = "";

        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            if ( i != 0 )
                s += "/";

            s += fields[i].GetValue(this).ToString();
        }

        return s;
    }

    public static EquipStats operator+(EquipStats es1, EquipStats es2){
        EquipStats es = new EquipStats();
        FieldInfo[] fields = es.GetType().GetFields();
        FieldInfo[] fields1 = es1.GetType().GetFields();
        FieldInfo[] fields2 = es2.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(es, (float)fields1[i].GetValue(es1) + (float)fields2[i].GetValue(es2));
        }
        return es;
    }
    public static EquipStats operator-(EquipStats es1, EquipStats es2){
        EquipStats es = new EquipStats();
        FieldInfo[] fields = es.GetType().GetFields();
        FieldInfo[] fields1 = es1.GetType().GetFields();
        FieldInfo[] fields2 = es2.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(es, (float)fields1[i].GetValue(es1) - (float)fields2[i].GetValue(es2));
        }
        return es;
    }
    public static EquipStats operator*(EquipStats es1, EquipStats es2){
        EquipStats es = new EquipStats();
        FieldInfo[] fields = es.GetType().GetFields();
        FieldInfo[] fields1 = es1.GetType().GetFields();
        FieldInfo[] fields2 = es2.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(es, (float)fields1[i].GetValue(es1) * (float)fields2[i].GetValue(es2));
        }
        return es;
    }
    public static EquipStats operator/(EquipStats es1, EquipStats es2){
        EquipStats es = new EquipStats();
        FieldInfo[] fields = es.GetType().GetFields();
        FieldInfo[] fields1 = es1.GetType().GetFields();
        FieldInfo[] fields2 = es2.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(es, (float)fields1[i].GetValue(es1) / (float)fields2[i].GetValue(es2));
        }
        return es;
    }
}
