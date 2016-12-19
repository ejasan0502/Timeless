using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientTest : MonoBehaviour {

    public GameObject playerPref;

    void Start(){
        Instantiate(playerPref);
    }

}
