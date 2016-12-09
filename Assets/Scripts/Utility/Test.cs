using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public GameObject playerPref;

    void Start(){
        Instantiate(playerPref);
    }

}
