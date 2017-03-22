using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles inventory UI logic such as equipping, consuming, stack splitting
public class InventoryUI : UI {

    public GameObject inventoryItemRef;
    public RectTransform content;
    public RectTransform menu;
    public GameObject equipBtn;
    public RectTransform info;
    public Inventory inventory;

    private List<RectTransform> inventoryItemUIs = new List<RectTransform>();

    private float x, y;
    private RectTransform rt, rt2;
    private int selectedIndex = 0;

    protected override void Awake(){
        base.Awake();

        if ( !content ) content = transform.GetChild(0).GetChild(0) as RectTransform;
        if ( !inventory ) {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }
        if ( !menu ){
            menu = transform.GetChild(2) as RectTransform;
            menu.gameObject.SetActive(false);
        }
        
        rt = transform as RectTransform;
        rt2 = inventoryItemRef.transform as RectTransform;

        key = KeyCode.I;
    }
    void OnEnable(){
        UpdateUI();

        SetInputControls(false);
    }
    void OnDisable(){
        SetMenuDisplay(-1,false);
        SetInputControls(true);
    }   

    // Create icon into content list
    private void CreateIcon(InventoryItem ii){
        GameObject o = (GameObject) Instantiate(inventoryItemRef, Vector3.zero, Quaternion.identity, content);
        o.transform.localEulerAngles = Vector3.zero;
        ((RectTransform)o.transform).anchoredPosition3D = new Vector3(x,y,-1f);
        
        x += rt2.rect.width;
        if ( x > rt.rect.width/2.00f ){
            x = -rt.rect.width/2.00f + rt2.rect.width/2.00f;
            y -= rt2.rect.height;
        }

        o.GetComponent<Image>().sprite = ii.item.Icon ?? (Sprite)Resources.Load(Settings.instance.path_icons+"default");
        o.transform.GetChild(0).GetComponent<Text>().text = ii.amount+"";

        InventoryItemUI iiu = o.GetComponent<InventoryItemUI>();
        iiu.inventoryUI = this;
        iiu.index = inventoryItemUIs.Count;

        inventoryItemUIs.Add(o.transform as RectTransform);
    }

    // Re-created grid list of contents
    public void UpdateUI(){
        if ( inventoryItemUIs.Count > 0 ){
            for (int i = inventoryItemUIs.Count-1; i >= 0; i--){
                Destroy(inventoryItemUIs[i].gameObject);
            }
            inventoryItemUIs = new List<RectTransform>();
        }
        
        x = -rt.rect.width/2.00f + rt2.rect.width/2.00f;
        y = -rt2.rect.height/2.00f;
        List<InventoryItem> items = inventory.Items;
        for (int i = 0; i < items.Count; i++){
            CreateIcon(items[i]);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, rt2.rect.height*Mathf.FloorToInt(inventoryItemUIs.Count/(rt.rect.width/rt2.rect.width)));
    }
    // Display/hide menu
    public void SetMenuDisplay(int index, bool b){
        if ( !menu ) return;

        if ( info ) info.gameObject.SetActive(false);
        menu.gameObject.SetActive(b);
        if ( b ){
            selectedIndex = index;
            menu.SetParent(inventoryItemUIs[selectedIndex]);
            menu.anchoredPosition3D = new Vector3(0,0,-1f);

            InventoryItem ii = inventory.GetInventoryItem(index);
            if ( ii != null ){
                equipBtn.SetActive(true);
                if ( ii.item.itemType == ItemType.equip ){
                    equipBtn.transform.GetChild(0).GetComponent<Text>().text = "Equip";
                } else if ( ii.item.itemType == ItemType.consumable ){
                    equipBtn.transform.GetChild(0).GetComponent<Text>().text = "Consume";
                } else {
                    equipBtn.SetActive(false);
                }
            }
        }
    }
    // Display/hide info
    public void SetInfoDisplay(int index, bool b){
        if ( !info ) return;

        info.gameObject.SetActive(b);
        if ( menu ) menu.gameObject.SetActive(false);
        if ( b ){
            selectedIndex = index;
            info.SetParent(inventoryItemUIs[selectedIndex]);
            info.anchoredPosition3D = new Vector3(info.rect.width/2.0f,-info.rect.height/2.0f,-1f);

            info.GetChild(0).GetComponent<Text>().text = inventory.GetInventoryItem(index).item.Description;
        }
    }
    // Execute if button1 on menu was pressed
    public void Menu_Apply(){
        InventoryItem ii = inventory.GetInventoryItem(selectedIndex);
        if ( ii != null ){
            if ( ii.item.itemType == ItemType.equip ){
                WeaponHandler weaponHandler = inventory.GetComponent<WeaponHandler>();
                if ( weaponHandler ){
                    Equip e = ii.item as Equip;
                    weaponHandler.AddWeapon(e.modelPath);

                    SetInfoDisplay(-1,false);
                    SetMenuDisplay(-1,false);
                    inventory.RemoveItem(ii.item,1);
                    UpdateUI();
                }
            } else if ( ii.item.itemType == ItemType.consumable ){
                
            }
        }
    }
    // Execute if button2 on menu was pressed
    public void Menu_Split(){

    }

}
