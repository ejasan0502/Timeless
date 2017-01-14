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
    private GrabTargets aoePoint = null;
    private int castSkillIndex = -1;

    void Awake(){
        character = GetComponent<Character>();
        view = GetComponent<NetView>();
        eventManager = EventManager.instance;

        UpdateUI();
    }
    void Start(){
        BirdEyeCameraControl becc = Camera.main.gameObject.AddComponent<BirdEyeCameraControl>();
        becc.SetFollow(transform);

        eventManager.AddEventHandler("OnSelectAoePoint", new System.EventHandler<MyEventArgs>(OnSelectAoePoint));
    }
    void Update(){
        Hotkeys();
        if ( Input.GetMouseButtonDown(0) ){
            if ( !UIManager.instance.InDeadZone(Input.mousePosition) ){
                RaycastHit hit;
                if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f) ){
                    if ( aoePoint != null ){
                        string[] targets = aoePoint.GetTargets("Enemy");
                        view.SendReliable("CastInput", RpcTarget.Server, castSkillIndex, targets);

                        Destroy(aoePoint.gameObject);
                        aoePoint = null;
                        castSkillIndex = -1;
                    } else if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ){
                        Move(hit.point);
                    } else if ( hit.collider.gameObject.layer == LayerMask.NameToLayer("Selectable") ){
                        Select(hit.collider.gameObject);
                    }
                }
            }
        }

        if ( aoePoint != null ){
            RaycastHit aoeHit;
            if ( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out aoeHit, 1000f) ){
                aoePoint.transform.position = aoeHit.point;
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

    public void OnSelectAoePoint(object sender, MyEventArgs args){
        try {
            castSkillIndex = (int)args.args[0];
            GameObject o = (GameObject) Instantiate(Resources.Load("aoeSphere"));
            aoePoint = o.GetComponent<GrabTargets>();
        } catch (Exception e){
            Debug.LogError(e.ToString());
        }
    }
}
