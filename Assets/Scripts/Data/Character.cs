using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles character object logic
public class Character : MonoBehaviour {

    [Header("-Character Info-")]
    public CharStats currentCharStats;
    public CombatStats currentCombatStats;

    public CharStats maxCharStats;
    public CombatStats maxCombatStats;

    [Header("-Graphics-")]
    public List<Texture> bloodyTextures;

    public bool IsAlive {
        get {
            return currentCharStats.health > 0;
        }
    }
    public bool IsPlayer {
        get {
            return tag == "Player" || tag == "User";
        }
    }
    public float MeleeDamage {
        get {
            return Random.Range(currentCombatStats.minMeleeDmg, currentCombatStats.maxMeleeDmg);
        }
    }
    public float RangeDamage {
        get {
            return Random.Range(currentCombatStats.minRangeDmg, currentCombatStats.maxRangeDmg);
        }
    }

    protected Animator anim;
    protected SkinnedMeshRenderer skinMeshRenderer;
    protected Material meshMat;

    private int bloodyTextureIndex = 0;

    protected virtual void Awake(){
        anim = GetComponent<Animator>();
        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        meshMat = new Material(skinMeshRenderer.sharedMaterial);
        skinMeshRenderer.sharedMaterial = meshMat;

        currentCharStats = new CharStats(maxCharStats);
        currentCombatStats = new CombatStats(maxCombatStats);

        StartCoroutine(RecoveryRate());
    }

    // Apply recovery on stats every 3 seconds
    private IEnumerator RecoveryRate(){
        while ( IsAlive ){
            yield return new WaitForSeconds(3f);
            if ( currentCharStats.health < maxCharStats.health ){
                currentCharStats.health += currentCharStats.hpRecov;
            }
            if ( currentCharStats.shields < maxCharStats.shields ){
                currentCharStats.shields += currentCharStats.shldRecov;
            }
            if ( currentCharStats.mana < maxCharStats.mana ){
                currentCharStats.mana += currentCharStats.mpRecov;
            }
            if ( currentCharStats.stamina < maxCharStats.stamina ){
                currentCharStats.stamina += currentCharStats.staRecov;
            }
        }
    }
    // Check if the character died
    private void CheckDeath(){
        if ( !IsAlive ){
            OnDeath();
        }
    }
    // Create a blood decal on the mesh
    private void ShowBlood(){
        if ( skinMeshRenderer && bloodyTextures.Count > 0 ){
            skinMeshRenderer.sharedMaterial.SetTexture("_DecalTex", bloodyTextures[bloodyTextureIndex]);
        }
    }

    // Do this upon death
    protected virtual void OnDeath(){

    }

    // Inflict damage to character
    public void Hit(Character atker, float rawDmg, InflictType inflictType){
        if ( !IsAlive ) return;


        float inflict = rawDmg;
        if ( inflictType == InflictType.melee || inflictType == InflictType.range ){
            inflict -= currentCombatStats.physDef;
        } else {
            inflict -= currentCombatStats.magicDef;
        }
        
        Debug.Log(string.Format("{0} receives {1} {2} damage from {3}", name, inflict, inflictType, atker.name));
        currentCharStats.health -= inflict;

        // If AI and has no target, set target to atker
        if ( !IsPlayer ){
            AI ai = this as AI;
            if ( !ai.HasTarget ){
                ai.SetTarget(atker);
            }
        }

        // Add blood effect if health threshold is reached
        if ( bloodyTextureIndex < bloodyTextures.Count && (bloodyTextureIndex+1)*(100f/bloodyTextures.Count+1) < 100f-100f*(currentCharStats.health/maxCharStats.health) ){
            bloodyTextureIndex++;
            ShowBlood();
        }

        CheckDeath();
    }
}
