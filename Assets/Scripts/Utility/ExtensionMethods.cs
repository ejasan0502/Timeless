using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

    public static Player GetSelf(this MonoBehaviour monoBehaviour){
        return GameManager.instance.player;
    }

    public static void Log(this MonoBehaviour monoBehaviour, string text){
        if ( GameManager.isSelf ){
            if ( GameManager.instance.debug ) Debug.Log(text);
        }
    }

}
