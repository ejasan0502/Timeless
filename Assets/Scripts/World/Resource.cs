using UnityEngine;
using System.Collections;

// A resource object in world
public class Resource : MonoBehaviour {

    public string resourceId;
    public int amount;
    public float maxHealth = 100f;

    private float health = 100f;
    private float prevHealth;

    void Awake(){
        health = maxHealth;
        prevHealth = health;
    }

    // Apply damage and return amount of resources based on damage applied
    public int Hit(float x){
        health -= x;

        float percent = (prevHealth/maxHealth) - (health/maxHealth);
        int amt = Mathf.RoundToInt(percent * amount);
        prevHealth = health;

        if ( health < 1 ){
            Destroy(gameObject);
            return amt;
        }
        return amt;
    }
}
