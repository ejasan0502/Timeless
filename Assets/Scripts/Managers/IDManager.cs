using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class IDManager : MonoBehaviour {

    private static List<string> generatedIds = new List<string>();

    public static string GenerateId(){
        string s = Guid.NewGuid().ToString();

        while ( generatedIds.Contains(s) ){
            s = Guid.NewGuid().ToString();
        }
        generatedIds.Add(s);

        return s;
    }

}
