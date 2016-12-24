using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryUI : MonoBehaviour, UI {

    public GameObject slotPrefab;
    public RectTransform content;
    public Text currencyText;

    public PlayerOwner player;
    private RectTransform scrollView;

    void Awake(){
        if ( !slotPrefab || !content || !currencyText ) {
            Debug.LogError("InventoryUI: Variables have not been set.");
            Destroy(this);
        }

        player = GameObject.FindWithTag("Player") ? GameObject.FindWithTag("Player").GetComponent<PlayerOwner>() : null;
        scrollView = (RectTransform) content.parent.parent ?? null;

        CreateGrid();
    }
    void OnEnable(){
        if ( player == null ){
            player = GameObject.FindWithTag("Player") ? GameObject.FindWithTag("Player").GetComponent<PlayerOwner>() : null;
        }
        UpdateAll();
    }

    private void CreateGrid(){
        RectTransform slotRT = (RectTransform)slotPrefab.transform;
        int width = Mathf.RoundToInt(scrollView.rect.width/slotRT.rect.width);
        int height = Mathf.RoundToInt(scrollView.rect.height/slotRT.rect.height);
        int maxSlots = width*height;

        if ( player != null && player.inventory.items.Count > maxSlots ){
            maxSlots = Mathf.CeilToInt(player.inventory.items.Count/width)*width;
        }

        float startX = scrollView.rect.min.x + slotRT.rect.width/2.00f;
        float x = startX;
        float y = scrollView.rect.max.y - slotRT.rect.height/2.00f;
        for (int i = 0; i < maxSlots; i++){
            GameObject o = Instantiate(slotPrefab);
            o.name = i+"";
            o.transform.SetParent(content);
            o.transform.localScale = Vector3.one;
            o.transform.position = scrollView.position + new Vector3(x,y,0f);
            o.AddComponent<InventorySlotUI>();

            UpdateSlot(i);

            x += slotRT.rect.width;
            if ( i != 0 && (i+1)%width == 0 ){
                x = startX;
                y -= slotRT.rect.height;
            }
        }
    }

    public string Id {
        get {
            return GetType().ToString();
        }
    }
    public MonoBehaviour Script {
        get {
            return this;
        }
    }
    public void SetDisplay(bool b){
        gameObject.SetActive(b);
    }
    
    public void UpdateAll(){
        for (int i = 0; i < content.childCount; i++){
            UpdateSlot(i);
        }
    }
    public void UpdateSlot(int index){
        Sprite icon = null;
        string amt = "";

        if ( player != null && index < player.inventory.items.Count ){
            icon = player.inventory.items[index].item.icon ?? Resources.Load<Sprite>("Icons/default");
            amt = player.inventory.items[index].amt > 1 ? player.inventory.items[index].amt+"" : "";
        }

        content.GetChild(index).GetChild(0).GetComponent<Image>().sprite = icon;
        content.GetChild(index).GetChild(1).GetComponent<Text>().text = amt;
    }
}
