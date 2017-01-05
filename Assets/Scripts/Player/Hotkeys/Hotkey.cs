using UnityEngine;
using System.Collections;

public abstract class Hotkey {

    public KeyCode key;
    public abstract void Apply();

    public Hotkey(){}

}
