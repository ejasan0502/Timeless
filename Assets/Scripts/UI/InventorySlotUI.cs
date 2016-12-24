using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventorySlotUI : EventTrigger {

    private Vector3 orgLocalPos;
    private bool canDrag = false;
    private PlayerOwner player;

    void Awake(){
        orgLocalPos = transform.localPosition;
        player = GameObject.FindWithTag("Player") ? GameObject.FindWithTag("Player").GetComponent<PlayerOwner>() : null;
    }
    void OnEnable(){
        if ( player == null ){
            player = GameObject.FindWithTag("Player") ? GameObject.FindWithTag("Player").GetComponent<PlayerOwner>() : null;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData){
        Debug.Log("OnBeginDrag");
        if ( player != null && int.Parse(name) < player.inventory.items.Count ){
            canDrag = true;
        }
    }
    public override void OnDrag(PointerEventData eventData){
        Debug.Log("OnDrag");
        if ( canDrag )
            transform.position = Input.mousePosition;
    }
    public override void OnEndDrag(PointerEventData eventData){
        Debug.Log("OnEndDrag");
        transform.localPosition = orgLocalPos;
        canDrag = false;
    }

}
