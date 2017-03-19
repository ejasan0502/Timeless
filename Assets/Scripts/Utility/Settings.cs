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
    public string anim_crouch = "isCrouching";
    public string anim_prone = "isProning";
    public string anim_weapon_type = "weaponType";
    public string anim_attack = "isAttacking";
    public string anim_reload = "isReloading";
    public string anim_death = "death";
    public string anim_swim = "isSwimming";

    [Header("-Camera Settings-")]
    public float cam_sensitivity = 5f;
    public float cam_minRotX = -75f, cam_maxRotX = 90f;
    public float defaultFov = 60f;

    private static Settings _instance;
    public static Settings instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<Settings>();
                if ( _instance == null ){
                    _instance = new GameObject("Settings").AddComponent<Settings>();
                }
            }
            return _instance;
        }
    }

    void Awake(){
        if ( instance != this )
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Environment"), LayerMask.NameToLayer("Water"));
    }
}
