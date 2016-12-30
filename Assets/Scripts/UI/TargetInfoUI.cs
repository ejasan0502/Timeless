using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetInfoUI : MonoBehaviour, UI {

    public Text nameText;
    public Image healthFillBar;
    public Image manaFillBar;
    private Character target = null;

    void Awake(){
        nameText = GetComponentInChildren<Text>();
        healthFillBar = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        manaFillBar = transform.GetChild(2).GetChild(0).GetComponent<Image>();
    }
    void LateUpdate(){
        if ( target != null ){
            if ( target.IsAlive ){
                healthFillBar.fillAmount = target.currentStats.hp/target.maxStats.hp;
                manaFillBar.fillAmount = target.currentStats.mp/target.maxStats.mp;
            } else {
                SetTarget(null);
            }
        }
    }

    public void SetTarget(Character c){
        nameText.text = c.name;
        target = c;
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

}
