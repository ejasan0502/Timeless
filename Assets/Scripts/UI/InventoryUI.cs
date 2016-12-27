using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Collections;

public class InventoryUI : MonoBehaviour, UI {

    public GameObject slotPrefab;
    public RectTransform content;
    public Text currencyText;
    public GameObject infoWindow;
    public Text infoText;

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
        content.sizeDelta = new Vector2(content.sizeDelta.x, (maxSlots/width)*slotRT.rect.height);

        float startX = slotRT.rect.width/2.00f;
        float x = startX;
        float y = -slotRT.rect.height/2.00f;
        for (int i = 0; i < maxSlots; i++){
            GameObject o = Instantiate(slotPrefab);
            o.name = i+"";
            o.transform.SetParent(content);
            o.transform.localScale = Vector3.one;
            o.transform.localPosition = new Vector3(x,y,0f);
            o.AddComponent<InventorySlotUI>();

            UpdateSlot(i);

            x += ((RectTransform)o.transform).rect.width;
            if ( i != 0 && (i+1)%width == 0 ){
                x = startX;
                y -= ((RectTransform)o.transform).rect.height;
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
    public void ShowInfo(int index){
        if ( player != null && index < player.inventory.items.Count ){
            infoWindow.SetActive(true);

            Vector3 pos = content.GetChild(index).position;
            infoWindow.transform.position = pos;

            Item item = player.inventory.items[index].item;
            string s = item.name + "\n";
            if ( item.itemType == ItemType.equip ){
                Equip e = item.GetAsEquip();
                s += e.equipType.ToString() + "\n";

                FieldInfo[] fields = e.stats.GetType().GetFields();
                foreach (FieldInfo fi in fields){
                    s += fi.Name + ": " + fi.GetValue(e.stats) + "\n";
                }
            } else if ( item.itemType == ItemType.usable ){
                s += "usable\n";
                s += item.description;
            } else {
                s += item.description;
            }

            infoText.text = s;
        }
    }
    public void HideInfo(){
        infoWindow.SetActive(false);
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
