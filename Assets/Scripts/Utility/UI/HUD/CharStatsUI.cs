using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Specfically handles the text used to update charStats
public class CharStatsUI : UI {

    public bool showAsPercentages;

    public Text healthText;
    public Text armorText;
    public Text shieldsText;
    public Text manaText;
    public Text staminaText;

    public float healthThreshold;
    public float armorThreshold;
    public float shieldsThreshold;
    public float manaThreshold;
    public float staminaThreshold;

    private Character player;

    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }
    void FixedUpdate(){
        if ( player ){
            if ( healthText ) healthText.text = "Health: " + (showAsPercentages ? (player.currentCharStats.health/player.maxCharStats.health)*100f+"%" : player.currentCharStats.health + " / " + player.maxCharStats.health);
            if ( armorText ) armorText.text = "Armor: " + (showAsPercentages ? (player.currentCharStats.armor/player.maxCharStats.armor)*100f+"%" : player.currentCharStats.armor + " / " + player.maxCharStats.armor);
            if ( shieldsText ) shieldsText.text = "Shields: " + (showAsPercentages ? (player.currentCharStats.shields/player.maxCharStats.shields)*100f+"%" : player.currentCharStats.shields + " / " + player.maxCharStats.shields);
            if ( manaText ) manaText.text = "Mana: " + (showAsPercentages ? (player.currentCharStats.mana/player.maxCharStats.mana)*100f+"%" : player.currentCharStats.mana + " / " + player.maxCharStats.mana);
            if ( staminaText ) staminaText.text = "Stamina: " + (showAsPercentages ? (player.currentCharStats.stamina/player.maxCharStats.stamina)*100f+"%" : player.currentCharStats.stamina + " / " + player.maxCharStats.stamina);

            HideTexts();
        }
    }

    // Hide irrelevant texts
    private void HideTexts(){
        if ( healthText ){
            if ( player.currentCharStats.health/player.maxCharStats.health <= healthThreshold ){
                if ( !healthText.IsActive() ){
                    healthText.gameObject.SetActive(true);
                }
            } else {
                if ( healthText.IsActive() ){
                    healthText.gameObject.SetActive(false);
                }
            }
        }

        if ( armorText ){
            if ( player.currentCharStats.armor/player.maxCharStats.armor <= armorThreshold ){
                if ( !armorText.IsActive() ){
                    armorText.gameObject.SetActive(true);
                }
            } else {
                if ( armorText.IsActive() ){
                    armorText.gameObject.SetActive(false);
                }
            }
        }

        if ( shieldsText ){
            if ( player.currentCharStats.shields/player.maxCharStats.shields <= shieldsThreshold ){
                if ( !shieldsText.IsActive() ){
                    shieldsText.gameObject.SetActive(true);
                }
            } else {
                if ( shieldsText.IsActive() ){
                    shieldsText.gameObject.SetActive(false);
                }
            }
        }

        if ( manaText ){
            if ( player.currentCharStats.mana/player.maxCharStats.mana <= manaThreshold ){
                if ( !manaText.IsActive() ){
                    manaText.gameObject.SetActive(true);
                }
            } else {
                if ( manaText.IsActive() ){
                    manaText.gameObject.SetActive(false);
                }
            }
        }

        if ( staminaText ){
            if ( player.currentCharStats.stamina/player.maxCharStats.stamina <= staminaThreshold ){
                if ( !staminaText.IsActive() ){
                    staminaText.gameObject.SetActive(true);
                }
            } else {
                if ( staminaText.IsActive() ){
                    staminaText.gameObject.SetActive(false);
                }
            }
        }
    }
}
