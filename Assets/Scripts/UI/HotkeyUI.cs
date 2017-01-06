using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HotkeyUI : MonoBehaviour {

    public Image icon;
    public Text text;
    public Image fill;

    void Awake(){
        text.text = "";
        fill.fillAmount = 0f;
    }
}
