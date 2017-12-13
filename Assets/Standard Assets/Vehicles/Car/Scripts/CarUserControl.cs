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


        private void Awake()
        {
        // get the car controller
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        }

        public void FullBrake()
        {
            CmdStop(vehicle.GetComponent<NetworkIdentity>().netId);
        }

        public void ResetBrake()
        {
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



        private void FixedUpdate()
        {
            vehicle = GameObject.Find("SnowMobile");
            if (Input.GetKeyUp(KeyCode.L))
            {
                CmdLights(vehicle.GetComponent<NetworkIdentity>().netId);
            }
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");
            handbrake = CrossPlatformInputManager.GetAxis("Jump");
        

            
            CmdMove(h, v, handbrake, vehicle.GetComponent<NetworkIdentity>().netId);


    }
    
}
