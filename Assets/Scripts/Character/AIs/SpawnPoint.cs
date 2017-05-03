using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Spawns a random monster at this transforms position
public class SpawnPoint : MonoBehaviour {

    public float spawnRate = 3;
    public int maxCount = 1;
    public GameObject[] monsterRefs;

    private List<GameObject> monsters = new List<GameObject>();

    IEnumerator Start(){
        if ( monsterRefs.Length < 1 ){
            this.Log("Spawn point does not contain any monster references.");
            yield break;
        }

        while (true){
            yield return new WaitForSeconds(spawnRate);
            if ( monsters.Count < maxCount ){
                GameObject o = Instantiate(monsterRefs[Random.Range(0,monsterRefs.Length)]);
                o.transform.position = transform.position;
                monsters.Add(o);
            }
        }
    }

}