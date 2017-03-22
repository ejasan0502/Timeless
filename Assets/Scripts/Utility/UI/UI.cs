using UnityEngine;
using System.Collections;

// Handles specific UI generic logic
public class UI : MonoBehaviour {

    public bool hideOnStart = false;
    public KeyCode key = KeyCode.None;
    protected UserInput user;

    protected virtual void Awake(){
        user = GameObject.FindWithTag("Player").GetComponent<UserInput>();
    }

    protected void SetInputControls(bool b){
        if ( user ){
            user.SetInputControls(b);
        }
    }
}
