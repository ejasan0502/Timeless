using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public static class Global {

    public const string PATH_MODELS_HUMANOIDS = "Models/Humanoids/";

    public static object Parse(Type type, string val){
        try {
            if ( type == typeof(ItemType) ) return Enum.Parse(type, val);
            else if ( type == typeof(EquipType) ) return Enum.Parse(type, val);
            else if ( type == typeof(EquipStats) ) return new EquipStats(val);
            else if ( type == typeof(CharStats) ) return new CharStats(val);
            else if ( type == typeof(float) ) return float.Parse(val);
            else if ( type == typeof(int) ) return int.Parse(val);
            else if ( type == typeof(string) ) return val;
        } catch (Exception e){
            Debug.LogError(e.ToString());
            return null;
        }

        Debug.LogError("Failed to parse type, " + type.ToString() + " of value, " + val);
        return null;
    }

}
