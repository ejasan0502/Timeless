using UnityEngine;
using System.Collections;
using MassiveNet;

[RequireComponent(typeof(NetView))]
public class MonsterSpawner : MonoBehaviour {

    public int maxCount = 1;

    private int count = 0;
    IEnumerator Start(){
        while (true){
            yield return new WaitForFixedUpdate();
            if ( count < maxCount ){
                Spawn();
            }
        }
    }

    private void Spawn(){
        count++;
        NetView view = Server.instance.NetViewManager.CreateView("Enemy");

        Character c = view.GetComponent<Character>();
        view.transform.position = transform.position;
        c.Move(transform.position);
    }
}
