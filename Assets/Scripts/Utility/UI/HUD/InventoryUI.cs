using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles inventory UI logic such as equipping, consuming, stack splitting
public class InventoryUI : UI {

    public GameObject inventoryItemRef;
    public RectTransform content;
    public Inventory inventory;

    private List<GameObject> inventoryItemUIs = new List<GameObject>();

    private float x, y;
    private RectTransform rt, rt2;
    public UserInput user;

    void Awake(){
        if ( !content ) content = transform.GetChild(0).GetChild(0) as RectTransform;
        if ( !inventory ) {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            if ( inventory ){
                user = inventory.GetComponent<UserInput>();
            }
        }
        
        rt = transform as RectTransform;
        rt2 = inventoryItemRef.transform as RectTransform;

        key = KeyCode.I;
    }
    void OnEnable(){
        UpdateUI();

        if ( user ){
            user.SetInputControls(false);
        }
    }
    void OnDisable(){
        if ( user ){
            user.SetInputControls(true);
        }
    }   

    private void CreateIcon(InventoryItem ii){
        GameObject o = (GameObject) Instantiate(inventoryItemRef, Vector3.zero, Quaternion.identity, content);
        o.transform.localEulerAngles = Vector3.zero;
        o.transform.localPosition = new Vector3(x,y,0f);
        
        x += rt2.rect.width;
        if ( x > rt.rect.width/2.00f ){
            x = -rt.rect.width/2.00f + rt2.rect.width/2.00f;
            y -= rt2.rect.height;
        }

        o.GetComponent<Image>().sprite = ii.item.Icon ?? (Sprite)Resources.Load(Settings.instance.path_icons+"default");
        o.transform.GetChild(0).GetComponent<Text>().text = ii.amount+"";

        inventoryItemUIs.Add(o);
    }

    public void UpdateUI(){
        if ( inventoryItemUIs.Count > 0 ){
            for (int i = inventoryItemUIs.Count-1; i >= 0; i--){
                Destroy(inventoryItemUIs[i]);
            }
            inventoryItemUIs = new List<GameObject>();
        }
        
        x = -rt.rect.width/2.00f + rt2.rect.width/2.00f;
        y = -rt2.rect.height/2.00f;

        List<InventoryItem> items = inventory.Items;
        for (int i = 0; i < items.Count; i++){
            CreateIcon(items[i]);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, rt2.rect.height*Mathf.FloorToInt(inventoryItemUIs.Count/(rt.rect.width/rt2.rect.width)));
    }

}
