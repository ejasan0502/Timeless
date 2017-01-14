using UnityEngine;
using System.Collections;

public abstract class Hotkey {
    
    public string iconPath;
    public KeyCode key;

    public Sprite Icon {
        get {
            return Resources.Load<Sprite>(iconPath) ?? Resources.Load<Sprite>("Icons/default");
        }
    }
    
    public Hotkey(){
        iconPath = "Icons/default";
        key = KeyCode.Alpha1;
    }
    public abstract void Apply();

}
