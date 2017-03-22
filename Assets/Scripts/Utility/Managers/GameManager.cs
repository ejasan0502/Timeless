using UnityEngine;
using System.Collections;

// Centralized script that initializes all non-monobehaviour scripts
public class GameManager : MonoBehaviour {

    private Settings settings;
    private ItemManager itemManager;

    void Awake(){
        if ( GameObject.FindObjectsOfType<GameManager>().Length > 1 ){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        InitializeScripts();
        PhysicsIgnoreCollision();
    }

    private void InitializeScripts(){
        settings = Settings.instance;
        itemManager = ItemManager.instance;
    }
    private void PhysicsIgnoreCollision(){
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Environment"), LayerMask.NameToLayer("Water"));
    }
}
