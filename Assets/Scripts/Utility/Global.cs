using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public static class Global {

    public static object Parse(Type type, string val){
        if ( type == typeof(ItemType) ) return Enum.Parse(type, val);
        else if ( type == typeof(EquipType) ) return Enum.Parse(type, val);
        else if ( type == typeof(EquipStats) ) return new EquipStats(val);
        else if ( type == typeof(CharStats) ) return new CharStats(val);
        else if ( type == typeof(float) ) return float.Parse(val);
        else if ( type == typeof(int) ) return int.Parse(val);
        else if ( type == typeof(string) ) return val;

        Debug.LogError("Failed to parse type, " + type.ToString() + " of value, " + val);
        return null;
    }

}
