using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;

public class CarUserControl : NetworkBehaviour
    {
        public GameObject vehicle; // the car controller we want to use
        private float h;
        private float v;
        private float handbrake;
        private bool mapActive = false;


        private void Awake()
        {
        // get the car controller
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        }

        public void FullBrake()
        {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        CmdStop(vehicle.GetComponent<NetworkIdentity>().netId);
        }

        public void ResetBrake()
        {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        CmdResetBrake(vehicle.GetComponent<NetworkIdentity>().netId);
        }

        [Command]
        private void CmdStop(NetworkInstanceId netId)
        {
            RpcStop(netId);
        }

        [ClientRpc]
        private void RpcStop(NetworkInstanceId netId)
        {
            GameObject car = ClientScene.FindLocalObject(netId);
            car.GetComponent<CarController>().SlowdownCar();
        }
        [Command]
        private void CmdResetBrake(NetworkInstanceId netId)
        {
            RpcResetBrake(netId);
        }

        [ClientRpc]
        private void RpcResetBrake(NetworkInstanceId netId)
        {
            GameObject car = ClientScene.FindLocalObject(netId);
            car.GetComponent<CarController>().BrakeReset();
        }


        [Command]
        private void CmdMove(float h, float v, float hb, NetworkInstanceId netId)
        {
            RpcMove(h, v, hb, netId);
        }
        [ClientRpc]
        private void RpcMove(float h, float v, float hb, NetworkInstanceId netId)
        {
            GameObject car = ClientScene.FindLocalObject(netId);
            car.GetComponent<CarController>().Move(h, v, v, hb);
        }

        [ClientRpc]
        private void RpcLights(NetworkInstanceId netId)
        {
            GameObject car = ClientScene.FindLocalObject(netId);
            if (car.GetComponent<CarController>()._lightsOn)
            {
                car.GetComponent<CarController>().SetLights(false);
            }
            else
            {
            car.GetComponent<CarController>().SetLights(true);
            }

        }
        [Command]
        private void CmdLights(NetworkInstanceId netId)
        {
            RpcLights(netId);
        }

    private void Update()
    {
        vehicle = GameObject.Find("SnowMobile");
        if (Input.GetKeyUp(KeyCode.L))
        {
            CmdLights(vehicle.GetComponent<NetworkIdentity>().netId);
        }
        if (Input.GetKeyDown(KeyCode.M) && !mapActive)
        {
            GameObject map = vehicle.transform.Find("VehMap").gameObject;
            map.SetActive(true);
            mapActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && mapActive)
        {
            GameObject map = vehicle.transform.Find("VehMap").gameObject;
            map.SetActive(false);
            mapActive = false;
        }


    }

    private void FixedUpdate()
        {
        vehicle = GameObject.Find("SnowMobile");

        h = CrossPlatformInputManager.GetAxis("Horizontal");
        v = CrossPlatformInputManager.GetAxis("Vertical");
        handbrake = CrossPlatformInputManager.GetAxis("Jump");



        CmdMove(h, v, handbrake, vehicle.GetComponent<NetworkIdentity>().netId);
    }
    
}
