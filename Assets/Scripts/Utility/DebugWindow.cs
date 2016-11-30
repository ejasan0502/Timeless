using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugWindow : MonoBehaviour {
    
    public Text text;
    public GameObject background;

    private Dictionary<LogType,string> textColors = new Dictionary<LogType,string>(){
        { LogType.Assert, "#ffffffff" },
        { LogType.Error, "#ff0000ff" },
        { LogType.Exception, "#ff0000ff" },
        { LogType.Log, "#ffffffff" },
        { LogType.Warning, "#ffff00ff" }
    };

    void Awake(){
        if ( GameObject.FindObjectsOfType<DebugWindow>().Length > 1 )
            Destroy(gameObject);

        if ( !text || !background ){
            Debug.LogError("Some variables are not set properly.");
            return;
        }

        text.text = "";
    }
    void Update(){
        if ( Input.GetKeyDown(KeyCode.Menu) ){
            background.SetActive(!background.activeSelf);
        }
    }
    void OnEnable(){
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable(){
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string message, string stackTrace, LogType type){
        if ( type == LogType.Error || type == LogType.Exception ){
            if ( !background.activeSelf ){
                background.SetActive(true);
            }
            text.text += "<color=" + textColors[type] + ">" + message + "\n" + stackTrace + "</color>\n";
        } else {
            text.text += "<color=" + textColors[type] + ">" + message + "</color>\n";
        }

        text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x,text.preferredHeight);
    }
}
