using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles UI for crafting
public class CraftingUI : UI {

    public GameObject craftItemUI;

    public RectTransform content;
    public Text infoText;
    public Transform craftingQueue;

    private CraftManager craftManager = null;
    private List<GameObject> craftItems = new List<GameObject>();
    private RectTransform rt;
    private RectTransform rt2;
    private int selectedIndex = -1;

    private static CraftingUI _instance;
    public static CraftingUI instance {
        get {
            if ( _instance == null ){
                _instance = GameObject.FindObjectOfType<CraftingUI>();
            }
            return _instance;
        }
    }

    protected override void Awake(){
        base.Awake();

        if ( CraftingUI.instance != this ) Destroy(gameObject);
        if ( !content ) content = transform.GetChild(0).GetChild(0) as RectTransform;
        if ( !infoText ) infoText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        if ( !craftingQueue ) craftingQueue = transform.GetChild(3);

        rt = transform as RectTransform;
        rt2 = craftItemUI.transform as RectTransform;
    }
    void OnEnable(){
        if ( !craftManager ){
            craftManager = GameObject.FindWithTag("Player").GetComponent<CraftManager>();
        }

        UpdateUI();
        SetInputControls(false);
    }
    void OnDisable(){
        craftManager = null;
        infoText.text = "";
        SetInputControls(true);
    }

    // Create the list of recipes, indicates which recipes are craftable
    private void UpdateUI(){
        if ( craftItems.Count > 0 ){
            for (int i = craftItems.Count-1; i >= 0; i--){
                Destroy(craftItems[i]);
            }
            craftItems = new List<GameObject>();
        }

        if ( craftManager != null ){
            float y = -rt2.rect.height/2.00f;
            for (int i = 0; i < craftManager.recipes.Count; i++){
                GameObject o = (GameObject) Instantiate(craftItemUI, Vector3.zero, Quaternion.identity, content);
                o.transform.localEulerAngles = Vector3.zero;
                ((RectTransform)o.transform).anchoredPosition3D = new Vector3(0f,y,-1f);

                Button btn = o.GetComponent<Button>();
                Text text = o.transform.GetChild(0).GetComponent<Text>();

                int x = i;
                btn.onClick.AddListener(() => SelectCraft(x));
                text.text = ItemManager.instance.GetItem(craftManager.recipes[i].productId).name;

                y -= rt2.rect.height;
                craftItems.Add(o);
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, rt2.rect.height*Mathf.FloorToInt(craftItems.Count/(rt.rect.width/rt2.rect.width)));
        }
    }
    // Update crafting queue ui
    private void UpdateCraftingUI(){
        for (int i = 0; i < craftingQueue.childCount; i++){
            Image icon = craftingQueue.GetChild(i).GetComponent<Image>();
            Text amtText = craftingQueue.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>();

            if ( i < craftManager.crafting.Count ){
                icon.sprite = ItemManager.instance.GetItem(craftManager.crafting[i].recipe.productId).Icon;
                amtText.text = craftManager.crafting[i].amount+"";
            } else {
                icon.sprite = null;
                amtText.text = "";
            }
        }
    }

    // Set the current craftManager
    public void SetCraftManager(CraftManager cm){
        craftManager = cm;
    }
    // Selecting a craft item
    public void SelectCraft(int index){
        if ( infoText ){
            selectedIndex = index;
            infoText.text = craftManager.recipes[index].Description;
        }
    }
    // Craft selected item
    public void Craft(){
        if ( selectedIndex != -1 ){
            if ( craftManager.CanCraft(selectedIndex) ){
                craftManager.AddCraftItem(selectedIndex,1);
                UpdateCraftingUI();
            } else {
                Debug.Log("Not enough materials to craft this item");
            }
        }
    }
    // Cancels a craft
    public void CancelCraft(int index){
        if ( index < craftManager.crafting.Count ){
            craftManager.Clear(index);
            UpdateCraftingUI();
        }
    }

}
