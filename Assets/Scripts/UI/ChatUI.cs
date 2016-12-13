using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatUI : MonoBehaviour, UI {

    public Text text;
    public InputField inputField;

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

    public void AddText(string s){
        if ( text == null ) return;

        text.text += "\n" + s;
        text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, text.preferredHeight);
    }
    public void SendText(){
        if ( inputField.text != "" ){
            Client.instance.Socket.Send("SendChatMessage", Client.instance.Server, Client.instance.Socket.Self.Endpoint.ToString(), inputField.text);
            inputField.text = "";
        }
    }
}
