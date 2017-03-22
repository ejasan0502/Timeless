using UnityEngine;
using System.Collections;

public class InventoryItemUI : MonoBehaviour {

    public int index;
    public InventoryUI inventoryUI;

    private bool displayInfo = false;

    void OnMouseOver(){
        if ( !displayInfo ){
            inventoryUI.SetInfoDisplay(index,true);
            displayInfo = true;
        }

        if ( Input.GetMouseButtonUp(1) ){
            inventoryUI.SetMenuDisplay(index,true);
        }
    }
    void OnMouseExit(){
        if ( displayInfo ){
            inventoryUI.SetInfoDisplay(index,false);
            displayInfo = false;
        }
    }

}
