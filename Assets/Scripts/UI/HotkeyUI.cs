using UnityEngine;
using System.Collections;

public class HotkeyUI : MonoBehaviour, UI {

    public string Id {
        get {
            return GetType().ToString();
        }
    }
    public MonoBehaviour Script {
        get {
            return this;
        }
    }
    public void SetDisplay(bool b){
        gameObject.SetActive(b);
    }

}
