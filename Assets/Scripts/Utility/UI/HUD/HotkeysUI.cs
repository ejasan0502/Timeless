using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Manages all hotkeyUIs
public class HotkeysUI : UI {

    public Sprite emptyHotkey;

    private List<HotkeyUI> hotkeys = new List<HotkeyUI>();

    private static HotkeysUI _instance;
    public static HotkeysUI instance {
        get {
            if ( _instance == null )
                _instance = GameObject.FindObjectOfType<HotkeysUI>();
            return _instance;
        }
    }

    void Awake(){
        if ( instance != this ) Destroy(gameObject);
    }
    void Start(){
        SetupHotkeys();
    }

    // Initialize hotkeys
    private void SetupHotkeys(){
        for (int i = 0; i < transform.childCount; i++){
            HotkeyUI hotkey = transform.GetChild(i).GetComponent<HotkeyUI>();

            hotkey.Set(emptyHotkey ?? null, "");

            hotkeys.Add(hotkey);
        }
    }

    // Return hotkey at specific point
    public HotkeyUI GetHotkeyAt(Vector3 point){
        foreach (HotkeyUI hotkey in hotkeys){
            if ( RectTransformUtility.RectangleContainsScreenPoint(hotkey.transform as RectTransform, (Vector2)Input.mousePosition, null) ){
                return hotkey;
            }
        }
        return null;
    }
}
