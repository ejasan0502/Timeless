using UnityEngine;
using System.Collections;

public class EquipTest : MonoBehaviour {
	
    void Start(){
        Equip e = ItemDatabase.instance.GetItem("rifle-0").GetAsEquip();
        GetComponent<Equipment>().Equip(e);
    }

}
