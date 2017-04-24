using UnityEngine;
using System.Collections;

public class InventoryItemUI : MonoBehaviour {

    public int index;
    public InventoryUI inventoryUI;
    public Vector3 originalPos;

    private bool displayInfo = false;
    private bool dragging = false;
    private float dragStartTime = 0f;

    void Update(){
        if ( dragging ){
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = inventoryUI.planeDist;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

            if ( Input.GetMouseButtonUp(0) ){
                dragging = false;
                transform.SetParent(inventoryUI.content);
                transform.SetSiblingIndex(index);
                ((RectTransform)transform).anchoredPosition3D = originalPos;
                
                if ( HotkeysUI.instance != null ){
                    HotkeyUI hotkey = HotkeysUI.instance.GetHotkeyAt(Input.mousePosition);
                    WeaponHandler weaponHandler = this.GetSelf().GetComponent<WeaponHandler>();
                    Inventory inventory = this.GetSelf().GetComponent<Inventory>();
                    InventoryItem inventoryItem = inventory.GetInventoryItem(index);
                    if ( inventoryItem.item.itemType == ItemType.block ){
                        ItemBlock itemBlock = inventoryItem.item as ItemBlock;
                        weaponHandler.AddWeapon(int.Parse(hotkey.name),itemBlock.modelPath);
                    } else if ( inventoryItem.item.itemType == ItemType.equip ){
                        Equip equip = inventoryItem.item as Equip;
                        if ( equip != null )
                            weaponHandler.AddWeapon(int.Parse(hotkey.name),equip.modelPath);
                        else
                            this.Log(inventoryItem.item.name + " does not derive from Equip");
                    }

                    hotkey.Set(inventoryItem.item.Icon, inventoryItem.amount+"");
                }
            }
        }
    }
    void OnMouseOver(){
        if ( !displayInfo && !dragging ){
            inventoryUI.SetInfoDisplay(index,true);
            displayInfo = true;
        }

        if ( Input.GetMouseButton(0) ){
            if ( !dragging && Time.time - dragStartTime >= 0.25f ){
                dragging = true;
                displayInfo = false;
                inventoryUI.SetInfoDisplay(index,false);
                transform.SetParent(inventoryUI.transform);
            }
        }

        if ( !dragging ){
            if ( Input.GetMouseButtonUp(1) ){
                inventoryUI.SetMenuDisplay(index,true);
            }
        }
    }
    void OnMouseExit(){
        if ( displayInfo ){
            inventoryUI.SetInfoDisplay(index,false);
            displayInfo = false;
        }
    }

}
