using UnityEngine;
using System.Collections;

// Centralized script that initializes all non-monobehaviour scripts
[RequireComponent(typeof(Settings))]
public class GameManager : MonoBehaviour {

    public bool debug = false;
    public Player player { get; private set; }
    public bool ignoreControlsInput { get; private set; }

    public static bool isDebugging {
        get {
            return GameManager.instance.debug;
        }
    }
    public static Camera_ThirdPerson Camera {
        get {
            if ( GameManager.instance.camera == null )
                GameManager.instance.camera = GameObject.FindObjectOfType<Camera_ThirdPerson>();

            return GameManager.instance.camera;
        }
    }

    private ItemManager itemManager;
    private Camera_ThirdPerson camera;

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

        GameObject playerObj = GameObject.FindWithTag("Player");
        if ( playerObj )
            player = playerObj.GetComponent<Player>();

        InitializeScripts();
        PhysicsIgnoreCollision();


    }

    // Initialize data scripts
    private void InitializeScripts(){
        itemManager = ItemManager.instance;
    }
    // Ignore physics for certain layers
    private void PhysicsIgnoreCollision(){
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Environment"), LayerMask.NameToLayer("Water"));
    }

    // Determine if system should ignore controls input
    public void SetControlsInput(bool ignore){
        ignoreControlsInput = ignore;
    }
    // Override default player found in Awake
    public void SetPlayer(Player player){
        this.player = player;
    }
}
