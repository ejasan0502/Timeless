﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles all UI elements
public class UIManager : MonoBehaviour {

    public UI[] uiElements;

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

        GatherUIElements();
    }

    // Fills uiElements list with UI components in children
    private void GatherUIElements(){
        uiElements = GetComponentsInChildren<UI>();
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
