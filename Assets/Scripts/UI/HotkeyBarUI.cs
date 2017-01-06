using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotkeyBarUI : MonoBehaviour, UI {

    public GameObject hotkeyPref;

    public List<HotkeyUI> hotkeys { get; private set; }

    void Awake(){
        CreateSlots();
    }

    private void CreateSlots(){
        RectTransform hotkeyRT = (RectTransform)hotkeyPref.transform;
        RectTransform rt = (RectTransform) transform;

        float startX = rt.rect.min.x + hotkeyRT.rect.width/2.00f;
        float x = startX;
        float y = 0f;

        hotkeys = new List<HotkeyUI>();
        for (int i = 0; i < 10; i++){
            GameObject o = (GameObject) Instantiate(hotkeyPref);
            o.name = i+"";
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(x,y,0f);
            o.transform.localScale = Vector3.one;
            hotkeys.Add(o.GetComponent<HotkeyUI>());

            x += hotkeyRT.rect.width;
        }
    }

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
