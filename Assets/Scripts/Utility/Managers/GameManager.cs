﻿using UnityEngine;
using System.Collections;

// Centralized script that initializes all non-monobehaviour scripts
[RequireComponent(typeof(Settings))]
public class GameManager : MonoBehaviour {

    public bool debug = false;
    public Player player { get; private set; }
    public bool ignoreControlsInput { get; private set; }

    public static bool isDebugging {
        get {
            return GameManager.instance.debug;
        }
    }
    public static CameraControl Camera {
        get {
            if ( GameManager.instance.cam == null )
                GameManager.instance.cam = GameObject.FindObjectOfType<CameraControl>();

            return GameManager.instance.cam;
        }
    }

    private ItemManager itemManager;
    private CameraControl cam;

    private static GameManager _instance;
    public static GameManager instance {
        get {
            if ( _instance == null )
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    void Awake(){
        if ( GameObject.FindObjectsOfType<GameManager>().Length > 1 ){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        GameObject playerObj = GameObject.FindWithTag("Player");
        if ( playerObj )
            player = playerObj.GetComponent<Player>();

        InitializeScripts();
        PhysicsIgnoreCollision();
    }

    // Initialize data scripts
    private void InitializeScripts(){
        itemManager = ItemManager.instance;
    }
    // Ignore physics for certain layers
    private void PhysicsIgnoreCollision(){
        
    }

    // Determine if system should ignore controls input
    public void SetControlsInput(bool ignore){
        ignoreControlsInput = ignore;
    }
    // Override default player found in Awake
    public void SetPlayer(Player player){
        this.player = player;
    }
}
