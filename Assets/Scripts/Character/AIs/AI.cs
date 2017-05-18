using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles generic AI logic
[RequireComponent(typeof(CharacterMovement))]
public class AI : Character {

    public float lookSpd = 2f;
    public float viewField = 0.5f;      // -1 to 1 where 1 is front and -1 is behind
    public float fireViewField = 0.9f;

    private AIState aiState = AIState.idle;

    protected CharacterMovement charMovt;
    protected AIRadius aiRadius;
    protected AudioSource audioSource;
    protected CharacterModel charModel;

    private Vector3 moveToPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private bool move = false;
    private bool combat = false;
    protected bool attack = false;
    protected int atkCounter = 0;

    protected bool canAttack = true;

    protected Character target = null;

    public bool HasTarget {
        get {
            return target != null;
        }
    }

    protected override void Awake(){
        base.Awake();

        charMovt = GetComponent<CharacterMovement>();
        aiRadius = GetComponentInChildren<AIRadius>();
        audioSource = GetComponent<AudioSource>();
        charModel = GetComponentInChildren<CharacterModel>();

        moveToPosition = transform.position;

        maxCharStats = new CharStats(Settings.instance.base_enemy_charStats);
        maxCombatStats = new CombatStats();
        currentCharStats = new CharStats(maxCharStats);
        currentCombatStats = new CombatStats(maxCombatStats);
    }
    void Update(){
        Movement();
    }
    void FixedUpdate(){
        StateMachine();
    }
    void LateUpdate(){
        if ( HasTarget && target.IsAlive ){
            Vector3 direction = transform.position - target.transform.position;
            Debug.DrawRay(charModel.spine1.position, direction*currentCombatStats.atkRange, Color.green);
            Quaternion lookRot = Quaternion.LookRotation(direction.normalized,transform.up);
            charModel.spine1.rotation = lookRot * charModel.spine1.rotation;
        }
    }

    // Handle AI states logic
    private void StateMachine(){
        if ( !IsAlive ) return;

        switch (aiState){
        case AIState.idle:
            Idle();
            break;
        case AIState.combat:
            Combat();
            break;
        case AIState.cast:
            Cast();
            break;
        }
    }
    // Handles AI movement
    private void Movement(){
        if ( move ){
            velocity = moveToPosition - transform.position;
            velocity.Normalize();

            charMovt.Move(velocity);

            Quaternion rot = Quaternion.LookRotation(velocity);
            rot.x = 0f;
            rot.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f*Time.deltaTime);

            if ( Vector3.Distance(transform.position, moveToPosition) < 1f ){
                Stop();
            }
        }

        Animate();
    }
    // Handles animations
    private void Animate(){
        anim.SetFloat(Settings.instance.anim_velocity_z, Mathf.Abs(velocity.z)*(combat ? 2f : 1f));
        anim.SetInteger(Settings.instance.anim_attack, atkCounter);
    }

    // Logic when AI has no target
    protected virtual void Idle(){
        if ( !HasTarget ){
            LookForTarget();   
        } else {
            SetState(AIState.combat);
        }
    }
    // Logic when AI has a target
    protected virtual void Combat(){
        if ( HasTarget && target.IsAlive ){
            if ( Vector3.Distance(transform.position, target.transform.position) < currentCombatStats.atkRange ){
                Stop();

                Vector3 direction = target.transform.position - transform.position;
                float dot = Vector3.Dot(direction.normalized, transform.forward);

                // Attack when target is in view
                if ( dot > fireViewField ){
                    RaycastHit hit;
                    // Check if attack has a clear line of sight
                    Debug.DrawRay(charModel.spine1.position, direction*currentCombatStats.atkRange, Color.green);
                    if ( Physics.Raycast(charModel.spine1.position, direction, out hit, currentCombatStats.atkRange) ){
                        if ( hit.collider.tag != "Untagged" ){
                            if ( canAttack ){
                                Attack();
                                canAttack = false;
                                StartCoroutine(AttackDelay());
                            }
                        } else {
                            // Do some logic
                        }
                    }
                }

                // Face target
                Quaternion lookRot = Quaternion.LookRotation(direction.normalized,transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, lookSpd*Time.deltaTime);
            } else if ( atkCounter < 1){
                MoveTo(target.transform.position);
            }
        } else {
            SetState(AIState.idle);
        }
    }
    // Logic when AI is casting
    protected virtual void Cast(){}

    // Attack based on type of weapon
    protected virtual void Attack(){
        if ( weaponHandler.HasWeaponEquipped ){
            Weapon weapon = weaponHandler.currentWeapons[0];
            
            if ( weapon.isFirearm ){
                Firearm firearm = weapon as Firearm;
                if ( firearm.autoFire ){
                    firearm.PrimaryFire();
                } else {
                    firearm.SinglePrimaryFire();
                }
            } else {
                weapon.SinglePrimaryFire();
            }
        }
    }
    
    // Looks for a desired target
    protected void LookForTarget(){
        if ( target != null ) return;
        
        Character desiredTarget = null;
        if ( aiRadius.enemiesWithinRange.Count > 0 ){
            float distance = 100f;

            foreach (Character c in aiRadius.enemiesWithinRange){
                Vector3 direction = c.transform.position - transform.position;
                float dot = Vector3.Dot(direction.normalized, transform.forward);

                // Check if in view
                if ( dot > viewField ){
                    // Look for closest
                    float d = Vector3.Distance(transform.position, c.transform.position);
                    if ( d < distance ){
                        desiredTarget = c;
                        distance = d;
                    }
                }
            }
        }
            
        SetTarget(desiredTarget);
    } 
    // Delay between attacks
    protected IEnumerator AttackDelay(){
        yield return new WaitForSeconds(currentCombatStats.atkRate);
        canAttack = true;
    }

    // Destroy object with delay on death
    protected override void OnDeath(){
        Stop();
        anim.SetBool(Settings.instance.anim_death, true);
        Destroy(gameObject, 10f);
    }
    // Move AI to position
    protected void MoveTo(Vector3 position){
        moveToPosition = position;
        move = true;
    }
    // Have AI stop moving
    protected void Stop(){
        move = false;
        velocity = Vector3.zero;
        charMovt.Move(velocity);
    }

    // Set current AI state to
    protected void SetState(AIState state){
        aiState = state;
    }

    // Set desired target
    public void SetTarget(Character c){
        target = c;

        if ( target != null ){
            combat = true;
        } else {
            combat = false;
        }
        
        charMovt.Sprint(combat);
    }
    // Set attack animation
    public void SetAtkCounter(int x){
        atkCounter = x;
    }
    // Play a sound
    public void PlaySound(AudioClip sound){
        if ( audioSource ){
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
    // Apply inflict damage to target
    public void ApplyDamage(){
        if ( attack ){
            if ( target != null && Vector3.Distance(transform.position, target.transform.position) < currentCombatStats.atkRange ){
                target.Hit(this, MeleeDamage, InflictType.melee);
            }
            attack = false;
        }
    }
}
