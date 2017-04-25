using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Component that initializes a UI element as a hotkey
public class HotkeyUI : MonoBehaviour {

    private Image image;
    private Text text;

    void Awake(){
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }

    // Set appearance of hotkey and save equipIndex of item
    public void Set(Sprite icon, string text){

        if ( this.image ) image.sprite = icon;
        if ( this.text ) this.text.text = text;
    }

}
