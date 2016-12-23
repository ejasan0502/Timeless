using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CharacterCreation : MonoBehaviour {

    public List<GameObject> characterObjs;

    private int viewIndex = 0;
    private Vector3 moveTo;

    void Awake(){
        moveTo = Camera.main.transform.position;
        if ( characterObjs.Count < 1 ){
            foreach (Transform t in transform){
                characterObjs.Add(t.gameObject);
            }
        }

        DontDestroyOnLoad(this);
    }
    void Update(){
        if ( Vector3.Distance(Camera.main.transform.position, moveTo) > 0f ){
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, moveTo, Time.deltaTime);
        }
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode){
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
        Client.instance.Socket.Send("SpawnRequest", Client.instance.Server);
        Destroy(gameObject);
    }

    public void ViewPrevious(){
        viewIndex--;
        if ( viewIndex < 0 ) viewIndex = characterObjs.Count-1;
        moveTo.x = characterObjs[viewIndex].transform.position.x;
    }
    public void ViewNext(){
        viewIndex++;
        if ( viewIndex >= characterObjs.Count ) viewIndex = 0;
        moveTo.x = characterObjs[viewIndex].transform.position.x;
    }
    public void Confirm(){
        Client.instance.Socket.Send("PlayerCreateRequest", Client.instance.Server, Global.PATH_MODELS_HUMANOIDS+characterObjs[viewIndex].name);
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("game");
    }
}
