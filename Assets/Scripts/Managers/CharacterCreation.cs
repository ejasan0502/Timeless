using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CharacterCreation : MonoBehaviour {

    public InputField nameField;

    void Awake(){
        DontDestroyOnLoad(this);
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode){
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
        Client.instance.Socket.Send("SpawnRequest", Client.instance.Server);
        Destroy(gameObject);
    }

    public void Confirm(){
        if ( nameField.text != "" ){
            Client.instance.Socket.Send("PlayerCreateRequest", Client.instance.Server, nameField.text, Global.PATH_MODELS_HUMANOIDS+"Default");
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            SceneManager.LoadScene("game");
        }
    }
}
