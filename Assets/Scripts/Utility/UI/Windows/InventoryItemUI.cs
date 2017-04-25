using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour {

    public int index;
    public InventoryUI inventoryUI;
    public Vector3 originalPos;

    private bool displayInfo = false;
    private bool dragging = false;
    private float dragStartTime = 0f;

    void Start(){
        AddListeners();
    }
    void Update(){
        if ( dragging ){
            transform.position = Input.mousePosition;
        }
    }

    private void AddListeners(){
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry;

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => {OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => {OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => {OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => {OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerEnter(PointerEventData data){
        this.Log("OnPointerEnter");
        inventoryUI.SetInfoDisplay(index,true);
        displayInfo = true;
    }
    public void OnPointerExit(PointerEventData data){
        this.Log("OnPointerExit");
        if ( displayInfo ){
            inventoryUI.SetInfoDisplay(index,false);
            displayInfo = false;
        }
    }
    public void OnBeginDrag(PointerEventData data){
        this.Log("OnBeginDrag");
        dragging = true;
        displayInfo = false;
        inventoryUI.SetInfoDisplay(index,false);
        transform.SetParent(inventoryUI.transform.parent);
    }
    public void OnEndDrag(PointerEventData data){
        this.Log("OnEndDrag");
        dragging = false;
        transform.SetParent(inventoryUI.content);
        transform.SetSiblingIndex(index);
        ((RectTransform)transform).anchoredPosition3D = originalPos;
                
        if ( HotkeysUI.instance != null ){
            HotkeyUI hotkey = HotkeysUI.instance.GetHotkeyAt(Input.mousePosition);
            WeaponHandler weaponHandler = this.GetSelf().GetComponent<WeaponHandler>();
            Inventory inventory = this.GetSelf().GetComponent<Inventory>();
            InventoryItem inventoryItem = inventory.GetInventoryItem(index);

            if ( hotkey == null ){
                this.Log("No hotkey found");
                return;
            }

            if ( inventoryItem.item.itemType == ItemType.block ){
                ItemBlock itemBlock = inventoryItem.item as ItemBlock;
                weaponHandler.AddWeapon(int.Parse(hotkey.name),itemBlock.modelPath);
            } else if ( inventoryItem.item.itemType == ItemType.equip ){
                Equip equip = inventoryItem.item as Equip;
                if ( equip != null ){
                    weaponHandler.AddWeapon(int.Parse(hotkey.name),equip.modelPath);
                } else
                    this.Log(inventoryItem.item.name + " does not derive from Equip");
            }

                    
            hotkey.Set(inventoryItem.item.Icon,inventoryItem.amount+"");
            inventory.Remove(inventoryItem);
        }
    }
}
