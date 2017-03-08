using UnityEngine;
using System.Reflection;
using System.Collections;

// Saves character stats relating to anything other than weapon
[System.Serializable]
public class CharStats {

    public float health;
    public float armor;
    public float shields;

    public float mana;
    public float stamina;

    public CharStats(){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, 0f);
        }
    }
    public CharStats(float val){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, val);
        }
    }
    public CharStats(CharStats val){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] fields2 = val.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this, fields2[i].GetValue(val));
        }
    }
}
