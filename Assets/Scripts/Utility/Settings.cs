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

    [Header("-World Settings-")]
    public float spawn_point_radius = 20f;
    public int maximum_vertices = 65000;

    [Header("Base Stat Settings")]
    public CharStats base_player_charStats;
    public CombatStats base_player_combatStats;
    public CharStats base_enemy_charStats;
    public CombatStats base_enemy_combatStats;

    [Header("-Resource Paths-")]
    public string path_icons = "Icons/";

    private static Settings _instance;
    public static Settings instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<Settings>();
            }
            return _instance;
        }
    }
}
