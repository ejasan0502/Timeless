using UnityEngine;
using System.Collections;

public class SkillUI : MonoBehaviour, UI {

    public GameObject skillSlotRef;

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
