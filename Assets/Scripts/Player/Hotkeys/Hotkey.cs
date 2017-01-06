using UnityEngine;
using System.Collections;

public abstract class Hotkey {
    
    public string iconPath;
    public KeyCode key;

    public Sprite Icon {
        get {
            return Resources.Load<Sprite>(iconPath);
        }
    }

    
    public Hotkey(){}
    public abstract void Apply();


}
