using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Specfically handles the text used to update charStats
public class CharStatsUI : UI {

    public bool ignoreThreshold;
    public bool displayTexts;
    public bool showAsPercentages;
    
    [System.Serializable]
    public class StatUI {
        public GameObject statObj;
        public Text text;
        public Image fill;
        public float threshold = 0.15f;
    }
    [SerializeField]
    public List<StatUI> statUIs = new List<StatUI>();

    private Character player;
    private List<StatUI> displayedUIs = new List<StatUI>();

    void Start(){
        SetupUIS();

        player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }
    void FixedUpdate(){
        if ( player ){
            for (int i = 0; i < statUIs.Count; i++){
                float current = 0f;
                float max = 1f;

                switch (i){
                case 0:
                    current = player.currentCharStats.health;
                    max = player.maxCharStats.health;
                    break;
                case 1:
                    current = player.currentCharStats.armor;
                    max = player.maxCharStats.armor;
                    break;
                case 2:
                    current = player.currentCharStats.shields;
                    max = player.maxCharStats.shields;
                    break;
                case 3:
                    current = player.currentCharStats.mana;
                    max = player.maxCharStats.mana;
                    break;
                case 4:
                    current = player.currentCharStats.stamina;
                    max = player.maxCharStats.stamina;
                    break;
                case 5:
                    current = player.currentCharStats.jetpack;
                    max = player.maxCharStats.jetpack;
                    break;
                }

                UpdateUI(statUIs[i], current, max);
            }
        }
    }

    // Update stat display, fill, and texts
    private void UpdateUI(StatUI statUI, float current, float max){
        if ( current/max <= statUI.threshold || ignoreThreshold ){
            if ( !statUI.statObj.activeSelf ){
                statUI.statObj.SetActive(true);
                if ( !displayedUIs.Contains(statUI) ){
                    displayedUIs.Add(statUI);
                    UpdateDisplayedUIs();
                }
            }

            if ( displayTexts && statUI.text ){
                statUI.text.text = showAsPercentages ? (Mathf.RoundToInt((current/max)*100f)+"%") : (Mathf.RoundToInt(current) + "/" + Mathf.RoundToInt(max));
            }
            if ( statUI.fill ){
                statUI.fill.fillAmount = current/max;
            }
        } else {
            if ( statUI.statObj.activeSelf ){
                statUI.statObj.SetActive(false);
                if ( displayedUIs.Contains(statUI) ){
                    displayedUIs.Remove(statUI);
                    UpdateDisplayedUIs();
                }
            }
        }
    }
    // Setup UIs from children transforms
    private void SetupUIS(){
        foreach (Transform t in transform){
            StatUI statUI = new StatUI();
            statUI.statObj = t.gameObject;
            statUI.fill = t.GetChild(1).GetComponent<Image>();
            statUI.text = t.GetChild(2).GetComponent<Text>();

            if ( t == transform.GetChild(transform.childCount-1) ){
                statUI.threshold = 0.99f;
            }

            statUIs.Add(statUI);
        }
    }
    // Update ui positions according to which uis is display
    private void UpdateDisplayedUIs(){
        if ( displayedUIs.Count < 1 ) return;

        float size = ((RectTransform)displayedUIs[0].statObj.transform).rect.width;

        for (int i = 0; i < displayedUIs.Count; i++){
            displayedUIs[i].statObj.transform.localPosition = new Vector3(0f, i*size, 0f);
        }
    }
}
