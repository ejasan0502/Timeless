using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

public class ServerTest : MonoBehaviour {

    public List<string> startItems = new List<string>(){
        "sword-0"
    };

    private Inventory inventory;

    void Awake(){
        inventory = GetComponent<Inventory>();
    }
    void Start(){
        foreach (string id in startItems){
            inventory.SendAdd(id, 1);
        }
    }
}
