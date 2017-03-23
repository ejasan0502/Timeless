using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles all UI elements
public class UIManager : MonoBehaviour {

    public UI[] uiElements;

    private UserInput user;

    private static UIManager _instance;
    public static UIManager instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    void Awake(){
        if ( instance != this ) Destroy(gameObject);

        DontDestroyOnLoad(this);

        user = GameObject.FindWithTag("Player").GetComponent<UserInput>();

        GatherUIElements();
    }
    void Start(){
        foreach (UI ui in uiElements){
            if ( ui.hideOnStart )
                Display(ui,false);
        }
    }
    void Update(){
        if ( uiElements.Length > 0 ){
            foreach (UI ui in uiElements){
                if ( ui.key != KeyCode.None ){
                    if ( Input.GetKeyUp(ui.key) ){
                        Display(ui,!ui.gameObject.activeSelf);
                    }
                }
            }

            if ( Input.GetKeyUp(KeyCode.Escape) ){
                foreach (UI ui in uiElements){
                    if ( ui.key != KeyCode.None ){
                        Display(ui,false);
                    }
                }
            }
        }
    }

    // Fills uiElements list with UI components in children
    private void GatherUIElements(){
        uiElements = GetComponentsInChildren<UI>();
    }
    // Show/Hide given ui element
    private void Display(UI ui, bool b){
        ui.gameObject.SetActive(b);

        if ( !b ){
            bool enableControls = true;
            foreach (UI uiElement in uiElements){
                if ( uiElement.hideOnStart && uiElement.gameObject.activeSelf ){
                    enableControls = false;
                }
            }

            if ( enableControls ){
                user.SetInputControls(true);
            }
        } else {
            user.SetInputControls(false);
        }
    }

    // Displays or hides a uiElement of given name
    public void SetView(string viewName, bool show){
        foreach (UI ui in uiElements){
            if ( ui.name == viewName ){
                ui.gameObject.SetActive(show);
            }
        }
    }
    // Display only uiElement of given name
    public void ShowAllExcept(string viewName){
        foreach (UI ui in uiElements){
            ui.gameObject.SetActive(ui.name == viewName);
        }
    }
}
