using UnityEngine;
using System.Collections;

// Centralized script that initializes all non-monobehaviour scripts
public class GameManager : MonoBehaviour {

    public Character player { get; private set; }

    private Settings settings;
    private ItemManager itemManager;

    private static GameManager _instance;
    public static GameManager instance {
        get {
            if ( _instance == null )
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    void Awake(){
        if ( GameObject.FindObjectsOfType<GameManager>().Length > 1 ){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        player = GameObject.FindWithTag("Player").GetComponent<Character>();

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
