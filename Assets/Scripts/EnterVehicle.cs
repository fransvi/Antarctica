using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Characters.FirstPerson;

public class EnterVehicle : NetworkBehaviour
{


    public Camera playerCam;
    public FirstPersonController FPSCon;
    public GameObject PlayerModel;
    public GameObject PlayerHealthBar;
    public GameObject PlayerColliders;
    public GameObject Player;
    [SyncVar]
    public GameObject Vehicle;
    public GameObject carParent;
    bool guiEnable = false;
    public Camera cam;
    public GameObject hands;
    bool inVehicle;
    bool nearVehicle;
    private NetworkIdentity objNetId;

    void Start()
    {


        inVehicle = false;
        playerCam = GetComponentInChildren<Camera>();
        FPSCon = transform.GetComponent<FirstPersonController>();
        PlayerModel = transform.Find("EMILY").gameObject;
        //hands = transform.Find("HANDS").gameObject;
        PlayerHealthBar = transform.Find("WorldSpaceHealthbarCanvas").gameObject;
        Player = this.gameObject;
        nearVehicle = false;

    }


    [Command]
    void CmdEnableModel()
    {
        RpcEnableModel();
    }
    [Command]
    void CmdDisableModel()
    {
        RpcDisableModel();
    }
    [ClientRpc]
    void RpcEnableModel()
    {
        PlayerModel.SetActive(true);
        //hands.SetActive(true);
        Vehicle.GetComponent<Rigidbody>().isKinematic = true;
    }
    [ClientRpc]
    void RpcDisableModel()
    {
        PlayerModel.SetActive(false);
        //hands.SetActive(false);
        Vehicle.GetComponent<Rigidbody>().isKinematic = false;
        Vehicle.GetComponent<Rigidbody>().WakeUp();
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

    void Update()
    {


        if (Vehicle == null)
        {
            Vehicle = GameObject.FindWithTag("Vehicle");
            //carParent = transform.Find("SnowMobile");
            cam = Vehicle.GetComponentInChildren<Camera>();
            cam.enabled = false;
        }
        if (inVehicle && Input.GetKeyUp(KeyCode.F))
        {
            //carParent = transform.Find("SnowMobile");
            if (cam.enabled == true)
            {

                if (carParent.GetComponent<CarSeats>()._hasDriver)
                {
                    //CmdRemoveObjectAuthority(GetComponent<NetworkIdentity>().netId);
                    Player = this.gameObject;
                    cam.enabled = false;
                    Player.GetComponent<FirstPersonController>().enabled = true;
                    PlayerModel.SetActive(true);
                    //PlayerHealthBar.SetActive(true);
                    playerCam.enabled = true;
                    Player.GetComponent<CarUserControl>().FullBrake();
                    Player.GetComponent<CarUserControl>().enabled = false;
                    carParent.GetComponent<CarController>()._hasDriver = false;
                    inVehicle = false;
                    //CmdEnableModel();
                }
                else
                {
                    //CmdRemoveObjectAuthority(GetComponent<NetworkIdentity>().netId);
                    Player = this.gameObject;
                    cam.enabled = false;
                    Player.GetComponent<FirstPersonController>().enabled = true;
                    PlayerModel.SetActive(true);
                    //PlayerHealthBar.SetActive(true);
                    playerCam.enabled = true;
                    Player.GetComponent<CarUserControl>().FullBrake();
                    Player.GetComponent<CarUserControl>().enabled = false;
                    carParent.GetComponent<CarController>()._hasPassanger = false;
                    inVehicle = false;
                    //CmdEnableModel();
                }

            }

        }
        else if (!inVehicle && Input.GetKeyUp(KeyCode.F))
        {
            //carParent = transform.Find("SnowMobile");
            if (cam.enabled == false)
            {
                Debug.Log(carParent);
                if (carParent.GetComponent<CarSeats>()._hasDriver)
                {
                    //CmdAssignObjectAuthority(GetComponent<NetworkIdentity>().netId);
                    Player = this.gameObject;
                    cam.enabled = true;
                    Player.GetComponent<FirstPersonController>().enabled = false;
                    PlayerModel.SetActive(false);
                    //PlayerHealthBar.SetActive(false);
                    playerCam.enabled = false;
                    guiEnable = false;
                    //Player.GetComponent<CarUserControl>().enabled = true;
                    carParent.GetComponent<CarController>()._hasPassanger = true;
                    inVehicle = true;
                    //CmdDisableModel();
                }
                else
                {

                    //CmdAssignObjectAuthority(GetComponent<NetworkIdentity>().netId);
                    Player = this.gameObject;
                    cam.enabled = true;
                    Player.GetComponent<FirstPersonController>().enabled = false;
                    PlayerModel.SetActive(false);
                    //PlayerHealthBar.SetActive(false);
                    playerCam.enabled = false;
                    guiEnable = false;
                    Player.GetComponent<CarUserControl>().enabled = true;
                    carParent.GetComponent<CarController>()._hasDriver = true;
                    inVehicle = true;
                    //CmdDisableModel();
                }

            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Vehicle")
        {

            if (inVehicle == false)
            {
                guiEnable = true;
                nearVehicle = true;
            }
            else
            {
                guiEnable = false;
                nearVehicle = false;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Vehicle")
        {
            guiEnable = false;
            nearVehicle = false;
        }
    }

    void OnGUI()
    {
        if (guiEnable != false)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), "F to Enter.");
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
        }
    }
}