using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MassiveNet;

public class InventorySlotUI : EventTrigger {

    private Vector3 orgLocalPos;
    private bool canDrag = false;
    private PlayerOwner player;
    private bool selected = false;

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
        if ( player != null && int.Parse(name) < player.inventory.items.Count ){
            InventoryUI inventoryUI = (InventoryUI) UIManager.instance.GetUI("InventoryUI").Script;
            inventoryUI.HideInfo();
            transform.SetAsLastSibling();
            canDrag = true;
        }
    }
    public override void OnDrag(PointerEventData eventData){
        if ( canDrag ){
            Vector3 newPos = Input.mousePosition;
            newPos.z = transform.position.z;
            transform.position = Input.mousePosition;
        }
    }
    public override void OnEndDrag(PointerEventData eventData){
        transform.SetSiblingIndex(int.Parse(name));
        transform.localPosition = orgLocalPos;
        canDrag = false;
    }
    public override void OnPointerEnter(PointerEventData eventData){
        if ( !canDrag ){
            InventoryUI inventoryUI = (InventoryUI) UIManager.instance.GetUI("InventoryUI").Script;
            inventoryUI.ShowInfo(int.Parse(name));
        }
    }
    public override void OnPointerExit(PointerEventData eventData){
        if ( !canDrag ){
            InventoryUI inventoryUI = (InventoryUI) UIManager.instance.GetUI("InventoryUI").Script;
            inventoryUI.HideInfo();
        }
    }
    public override void OnPointerClick(PointerEventData eventData){
        if ( selected ){
            selected = false;
            if ( player.inventory.items[int.Parse(name)].item.itemType == ItemType.equip ){
                player.view.SendReliable("EquipRequest", RpcTarget.Server, int.Parse(name));
            }
        } else {
            selected = true;
        }
    }
}
