using UnityEngine;
using System.Collections;

// All settings for the game
public class Settings {

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

    [Header("-World Settings-")]
    public float spawn_point_radius = 20f;
    public int maximum_vertices = 65000;

    [Header("-Resource Paths-")]
    public string path_icons = "Icons/";

    private static Settings _instance;
    public static Settings instance {
        get {
            if ( _instance == null ){
                _instance = new Settings();
            }
            return _instance;
        }
    }

    public Settings(){}
}
