using UnityEngine;
using System.Reflection;
using System.Collections;

public class CharStats {
    
    public float hp;
    public float mp;
    public float sta;

    public float hpRecov;       // Percent
    public float mpRecov;       // Percent
    public float staRecov;      // Percent

    public float acc;           // Percent
    public float eva;           // Percent
    public float movtSpd;

    public CharStats(){
        foreach (FieldInfo fi in GetType().GetFields()){
            fi.SetValue(this, 0f);
        }
    }
    public CharStats(float val){
        foreach (FieldInfo fi in GetType().GetFields()){
            fi.SetValue(this, val);
        }
    }
    public CharStats(CharStats cs){
        FieldInfo[] fields1 = GetType().GetFields();
        FieldInfo[] fields2 = cs.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields1[i].SetValue(this, (float)fields2[i].GetValue(cs));
        }
    }
    public CharStats(string s){
        string[] args = s.Split('/');
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Length; i++){
            fields[i].SetValue(this, float.Parse(s));
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

    public static CharStats operator+(CharStats cs1, CharStats cs2){
        CharStats cs = new CharStats();
        FieldInfo[] fields = cs.GetType().GetFields();
        FieldInfo[] fields1 = cs1.GetType().GetFields();
        FieldInfo[] fields2 = cs2.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields[i].SetValue(cs, (float)fields1[i].GetValue(cs1) + (float)fields2[i].GetValue(cs2));
        }
        return cs;
    }
    public static CharStats operator-(CharStats cs1, CharStats cs2){
        CharStats cs = new CharStats();
        FieldInfo[] fields = cs.GetType().GetFields();
        FieldInfo[] fields1 = cs1.GetType().GetFields();
        FieldInfo[] fields2 = cs2.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields[i].SetValue(cs, (float)fields1[i].GetValue(cs1) - (float)fields2[i].GetValue(cs2));
        }
        return cs;
    }
    public static CharStats operator*(CharStats cs1, CharStats cs2){
        CharStats cs = new CharStats();
        FieldInfo[] fields = cs.GetType().GetFields();
        FieldInfo[] fields1 = cs1.GetType().GetFields();
        FieldInfo[] fields2 = cs2.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields[i].SetValue(cs, (float)fields1[i].GetValue(cs1) * (float)fields2[i].GetValue(cs2));
        }
        return cs;
    }
    public static CharStats operator/(CharStats cs1, CharStats cs2){
        CharStats cs = new CharStats();
        FieldInfo[] fields = cs.GetType().GetFields();
        FieldInfo[] fields1 = cs1.GetType().GetFields();
        FieldInfo[] fields2 = cs2.GetType().GetFields();
        for (int i = 0; i < fields1.Length; i++){
            fields[i].SetValue(cs, (float)fields1[i].GetValue(cs1) / (float)fields2[i].GetValue(cs2));
        }
        return cs;
    }
}
