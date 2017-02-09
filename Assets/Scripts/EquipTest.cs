using UnityEngine;
using System.Collections;

public class EquipTest : MonoBehaviour {
	
    private CharacterModel charModel;

    void Awake(){
        charModel = GetComponent<CharacterModel>();
    }
    void Start(){
        GameObject o = (GameObject)Instantiate(Resources.Load("Models/Equipment/Weapons/Guns/Rifles/Ak47"));

        Vector3 pos = o.transform.position;
        Quaternion rot = o.transform.rotation;

        o.transform.SetParent(charModel.nodes[(int)EquipType.primary]);
        o.transform.localPosition = pos;
        o.transform.localRotation = rot;

        foreach (Transform t in o.transform){
            t.gameObject.layer = LayerMask.NameToLayer("Limbs");
        }
    }

}
