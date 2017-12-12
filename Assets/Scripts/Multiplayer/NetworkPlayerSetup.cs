﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerSetup : NetworkBehaviour
{

    public Behaviour[] componentsToEnable;
    public Camera playerCamera;
    public GameObject playerUi;
    public int playerHealth;
    public bool inVehicle;
    public GameObject worldModel;
    public GameObject viewModel;
    public GameObject weatherSystem;
    public bool isClients;
    public bool isServers;
    [SyncVar]
    public string pName = "player";

    [SyncVar]
    public Color playerColor = Color.white;

    void Update()
    {
        isClients = isClient;
        isServers = isServer;
        //Might cause lag TODO: better implementation
        if (inVehicle)
        {
            
        }

        
    }


    void Start()
    {

        //Placeholder :: Set colour for player 
        weatherSystem = GameObject.Find("WeatherSystem");
        Renderer[] rend = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend)
        {
            //r.material.color = playerColor;
        }
        if (!isLocalPlayer)
        {
            weatherSystem.SetActive(false);
            this.GetComponent<CharacterController>().enabled = false;
            viewModel.SetActive(false);

        }
    }

    public void ReloadWorldModels()
    {
        if (isLocalPlayer)
        {
            worldModel.SetActive(false);
        }
        else
        {
            worldModel.SetActive(true);
        }
    }

    public void DisableWorldModel()
    {
        worldModel.SetActive(false);
    }



    public override void OnStartLocalPlayer()
    {

        gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
        playerCamera = GetComponentInChildren<Camera>();
        //Enable Components
        foreach (Behaviour component in componentsToEnable)
        {
            component.enabled = true;
        }
        worldModel.SetActive(false);
        //Enable Camera
        playerCamera.enabled = true;
        playerCamera.GetComponent<AudioListener>().enabled = true;
        //Enable CharacterController
        this.GetComponent<CharacterController>().enabled = true;

        //Renderer[] rens = GetComponentsInChildren<Renderer>();
        //foreach(Renderer ren in rens)
        //{
        //    ren.enabled = false;
        //}

        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);

 
    }

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

    public void Init()
    {
        //Player init through Gamemanager
    }

    public void EnableLobbyCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    void Awake()
    {
        NetworkGameManager.players.Add(this);
    }

    void OnDestroy()
    {
        NetworkGameManager.players.Remove(this);
    }



}
