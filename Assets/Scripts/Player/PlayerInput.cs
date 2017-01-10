using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MassiveNet;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {

    private Hotkey[] hotkeys = new Hotkey[10]{
        new AttackHotkey(), null, null, null, null,
        null, null, null, null, null
    };
    private Character character;
    private NetView view;
    private EventManager eventManager;
    private Skill castSkill = null;

    void Awake(){
        character = GetComponent<Character>();
        view = GetComponent<NetView>();
        eventManager = EventManager.instance;

        UpdateUI();
    }
    void Start(){
        BirdEyeCameraControl becc = Camera.main.gameObject.AddComponent<BirdEyeCameraControl>();
        becc.SetFollow(transform);

        eventManager.AddEventHandler("OnTargetSelect", new System.EventHandler<MyEventArgs>(OnTargetSelect));
        eventManager.AddEventHandler("OnCastCancel", new System.EventHandler<MyEventArgs>(OnCastCancel));
        eventManager.AddEventHandler("OnCastEnd", new System.EventHandler<MyEventArgs>(OnCastEnd));
    }
    void Update(){
        Hotkeys();
        if ( Input.GetMouseButtonDown(0) ){
            if ( !UIManager.instance.InDeadZone(Input.mousePosition) ){
                RaycastHit hit;
                if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f) ){
                    if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ){
                        Move(hit.point);
                    } else if ( castSkill != null ){
                        Cast(hit);
                    } else if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Selectable") ){
                        Select(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void Move(Vector3 moveTo){
        Instantiate(Resources.Load("Effects/MovePointer"), moveTo, Quaternion.identity);

        character.Move(moveTo);
        view.SendReliable("MoveInput", RpcTarget.Server, moveTo);
    }
    private void Select(GameObject o){
        Character c = o.GetComponent<Character>();
        if ( c != null ){
            UI targetInfo = UIManager.instance.GetUI("TargetInfoUI");
            if ( targetInfo != null ){
                character.SetTarget(c);
                view.SendReliable("SetTargetInput", RpcTarget.Server, c.id);

                targetInfo.SetDisplay(true);
                TargetInfoUI targetInfoUI = (TargetInfoUI) targetInfo;
                targetInfoUI.SetTarget(c);
            }
        }
    }
    private void Hotkeys(){
        foreach (Hotkey hotkey in hotkeys){
            if ( hotkey != null ){
                if ( Input.GetKeyDown(hotkey.key) ){
                    hotkey.Apply();
                }
            }
        }
    }
    private void Cast(RaycastHit hit){
        switch (castSkill.aoeType){
        default: Debug.Log("Player input does not implement aoeType, " + castSkill.aoeType.ToString()); break;
        #region Single Target
        case AoeType.singleTarget:
        if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Selectable") ){
            Character c = hit.collider.gameObject.GetComponent<Character>();
            if ( c != null ){
                if ( !castSkill.friendly && c.tag == "Enemy" || castSkill.friendly && c.tag == "Player" ){
                    Select(hit.collider.gameObject);
                    if ( castSkill.IsInstant ){
                        castSkill.Apply(new List<Character>(){character.Target});
                    } else {
                        character.SetAnimState("castAnim", (int)castSkill.castAnim);
                    }
                } else {
                    Debug.Log("Invalid target.");
                }
            } else {
                Debug.Log("Invalid target.");
            }
        }
        break;
        #endregion;
        }
    }

    public void UpdateHotkeyUI(int index){
        HotkeyUI hotkeyUI = ((HotkeyBarUI)UIManager.instance.GetUI("HotkeyBarUI").Script).hotkeys[index];
        Hotkey h = hotkeys[index];

        hotkeyUI.icon.sprite = h.Icon;
        hotkeyUI.fill.fillAmount = 0f;
        hotkeyUI.text.text = "";
    }
    public void UpdateUI(){
        for (int i = 0; i < hotkeys.Length; i++){
            if ( hotkeys[i] != null ){
                UpdateHotkeyUI(i);
            }
        }
    }

    public void OnTargetSelect(object sender, MyEventArgs args){
        try {
            castSkill = (Skill)args.args[0];
        } catch (Exception e){
            Debug.LogError(e.ToString());
        }
    }
    public void OnCastCancel(object sender, MyEventArgs args){
        
    }
    public void OnCastEnd(object sender, MyEventArgs args){
        
    }
}
