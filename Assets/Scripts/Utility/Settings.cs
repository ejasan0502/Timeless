using UnityEngine;
using System.Collections;

// All settings for the game
public class Settings : MonoBehaviour {

    [Header("-Animator Settings-")]
    public string anim_velocity_x = "velX";
    public string anim_velocity_z = "velZ";
    public string anim_grounded = "isGrounded";
    public string anim_jump = "isJumping";
    public string anim_free_fall = "isFreeFalling";
    public string anim_dodge = "isDodging";

    [Header("-Camera Settings-")]
    public float cam_sensitivity = 5f;
    public float cam_minRotX = -75f, cam_maxRotX = 90f;

    private static Settings _instance;
    public static Settings instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<Settings>();
            }
            return _instance;
        }
    }

    void Awake(){
        if ( instance != this )
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
}
