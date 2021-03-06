﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class EnterVehicle : NetworkBehaviour
{


    public Camera _playercam;
    public FirstPersonController _fpsCon;
    public GameObject _playerModel;
    public GameObject _playerHealthBar;
    public GameObject _playerColliders;
    public GameObject _player;
    [SyncVar]
    public GameObject Vehicle;
    public GameObject _carParent;
    public GameObject _handsModel;
    bool guiEnable = false;
    public Camera cam;
    bool _inVehicle;
    bool _nearVehicle;
    public GameObject _spawnPoint;
    private NetworkIdentity _objNetId;
    private bool _start = true;

    void Start()
    {


        _inVehicle = false;

        _playercam = GetComponentInChildren<Camera>();
        _fpsCon = transform.GetComponent<FirstPersonController>();
        _playerModel = transform.Find("EMILY").gameObject;
        //_handsModel = transform.Find("HANDS").gameObject;
        _playerHealthBar = transform.Find("WorldSpaceHealthbarCanvas").gameObject;
        _player = this.gameObject;
        _nearVehicle = false;
    }



    [Command]
    void CmdModelEnable(NetworkInstanceId netId, bool b)
    {
        RpcModelEnable(netId, b);
    }

    [ClientRpc]
    void RpcModelEnable(NetworkInstanceId netId, bool b)
    {


        
        
        GameObject obj = ClientScene.FindLocalObject(netId);
        if (b)
        {
            obj.GetComponent<NetworkPlayerSetup>().ReloadWorldModels();
        }
        else
        {
            obj.GetComponent<NetworkPlayerSetup>().DisableWorldModel();
        }



        /*
        Transform[] transforms = obj.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            Debug.Log(t.name);
            if (t.name == "EMILY")
            {
                if (b)
                {
                    t.gameObject.SetActive(true);
                }
                else
                {
                    t.gameObject.SetActive(false);
                }

            }
        }
        */
        
        
    }
  
    [Command]
    void CmdAssignObjectAuthority(NetworkInstanceId netInstanceId)
    {
        // Assign authority of this objects network instance id to the client
        NetworkServer.objects[netInstanceId].AssignClientAuthority(connectionToClient);
    }
    [Command]
    void CmdRemoveObjectAuthority(NetworkInstanceId netInstanceId)
    {
        // Removes the  authority of this object network instance id to the client
        NetworkServer.objects[netInstanceId].RemoveClientAuthority(connectionToClient);
    }

    private void ActivateVehicle(bool b)
    {
        if (_nearVehicle)
        {
            _player = this.gameObject;
            cam = _carParent.GetComponentInChildren<Camera>();
            cam.enabled = true;
            _player.GetComponent<FirstPersonController>().enabled = false;
            _player.GetComponent<CharacterController>().enabled = false;
            //_playerModel.SetActive(false);
            _handsModel.SetActive(false);

            RpcModelEnable(_player.GetComponent<NetworkIdentity>().netId, false);

            _playercam.enabled = false;
            guiEnable = false;
            _player.GetComponent<CarUserControl>().ResetBrake();

            if (b)
            {
                _carParent.GetComponent<CarController>()._hasPassanger = true;
            }
            else
            {
                _player.GetComponent<CarUserControl>().enabled = true;
                _carParent.GetComponent<CarController>()._hasDriver = true;
            }

            _inVehicle = true;
            _carParent.GetComponent<CarController>().setWeather(true);
            
        }

    }
    private void DeactivateVehicle(bool b)
    {
        _player = this.gameObject;
        cam = _carParent.GetComponentInChildren<Camera>();
        cam.enabled = false;
        _player.GetComponent<FirstPersonController>().enabled = true;
        _player.GetComponent<CharacterController>().enabled = true;

        _handsModel.SetActive(true);
        
        RpcModelEnable(_player.GetComponent<NetworkIdentity>().netId, true);

        
        GetComponent<NetworkPlayerSetup>().ReloadWorldModels();
        //_playerModel.SetActive(false);
        _playercam.enabled = true;
        _player.GetComponent<CarUserControl>().FullBrake();
        _player.GetComponent<CarUserControl>().enabled = false;
        if (b)
        {
            _carParent.GetComponent<CarController>()._hasDriver = false;
        }
        else
        {
            _carParent.GetComponent<CarController>()._hasPassanger = false;
        }

        _inVehicle = false;

        Transform exitPoint = _carParent.transform.Find("ExitPoint");

        transform.position = exitPoint.position;
        transform.rotation = exitPoint.rotation;
        _carParent.GetComponent<CarController>().setWeather(false);
        
        //_playerModel.SetActive(false);

    }

    void Update()
    {
        // Debug.Log("2: " + isServer);
        if (_start)
        {
            _carParent = GameObject.Find("SnowMobile");
            cam = _carParent.GetComponentInChildren<Camera>();
            cam.enabled = false;
            _start = false;
        }



        //Exit vehicle
        if (_inVehicle && Input.GetKeyUp(KeyCode.F))
        {
            _carParent = GameObject.Find("SnowMobile");

                if (_carParent.GetComponent<CarSeats>()._hasDriver)
                {
                    //Deactivate driver seat
                    DeactivateVehicle(true);
                }
                else
                {
                    //Deacivate passanger seat
                    DeactivateVehicle(false);
                }


        }

        //Enter vehicle
        else if (!_inVehicle && Input.GetKeyUp(KeyCode.F))
        {
                if (_carParent.GetComponent<CarSeats>()._hasDriver)
                {
                    //Activate passanger seat
                    ActivateVehicle(true);
                }
                else
                {
                    //Activate driver seat
                    ActivateVehicle(false);
                }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Vehicle")
        {

            if (_inVehicle == false)
            {
                guiEnable = true;
                _nearVehicle = true;
            }
            else
            {
                guiEnable = false;
                _nearVehicle = false;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Vehicle")
        {
            guiEnable = false;
            _nearVehicle = false;
        }
    }

    void OnGUI()
    {
        if (guiEnable != false)
        {
            //_player = this.gameObject;
            //_player.GetComponent<RaycastShooting>()._textImage.gameObject.SetActive(true);
            //_player.GetComponent<RaycastShooting>()._textImage.gameObject.GetComponentInChildren<Text>().text = "F to Enter.";
            //_player.GetComponent<RaycastShooting>()._textImage.gameObject.GetComponentInChildren<Text>().resizeTextForBestFit = true;
            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), "F to Enter.");
        }
        else
        {
            _player = this.gameObject;
            //_player.GetComponent<RaycastShooting>()._textImage.gameObject.SetActive(false);
            //_player.GetComponent<RaycastShooting>()._textImage.gameObject.GetComponentInChildren<Text>().resizeTextForBestFit = false;
            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
        }
    }
}