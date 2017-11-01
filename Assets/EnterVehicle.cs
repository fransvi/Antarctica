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
    [SyncVar]
    public Transform carParent;
    bool guiEnable = false;
    public Camera cam;
    bool inVehicle;
    bool nearVehicle;

    void Start()
    {


        inVehicle = false;
        playerCam = GetComponentInChildren<Camera>();
        FPSCon = transform.GetComponent<FirstPersonController>();
        PlayerModel = transform.Find("emilyvalmis").gameObject;
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
        Vehicle.GetComponent<Rigidbody>().isKinematic = true;
        Vehicle.gameObject.GetComponent<CarUserControl>().enabled = false;
    }
    [ClientRpc]
    void RpcDisableModel()
    {
        PlayerModel.SetActive(false);
        Vehicle.GetComponent<Rigidbody>().isKinematic = false;
        Vehicle.GetComponent<Rigidbody>().WakeUp();
        Vehicle.gameObject.GetComponent<CarUserControl>().enabled = true;
    }
    
    void Update()
    {
        if(Vehicle == null)
        {
            Vehicle = GameObject.FindWithTag("Vehicle");
            carParent = transform.Find("SnowMobile");
            cam = Vehicle.GetComponentInChildren<Camera>();
            cam.enabled = false;
        }
        if (!isLocalPlayer)
        {
            return;
        }
        if (inVehicle && Input.GetKeyUp(KeyCode.F))
        {
            
            if (cam.enabled == true)
            {
                cam.enabled = false;
                Player.GetComponent<FirstPersonController>().enabled = true;
                PlayerModel.SetActive(true);
                Player.transform.parent = null;
                PlayerHealthBar.SetActive(true);
                playerCam.enabled = true;
                Vehicle.gameObject.GetComponent<CarUserControl>().enabled = false;             
                inVehicle = false;
                CmdEnableModel();

            }
            
        }else if(!inVehicle && Input.GetKeyUp(KeyCode.F))
        {
            if (cam.enabled == false)
            {
                cam.enabled = true;
                Player.GetComponent<FirstPersonController>().enabled = false;
                PlayerModel.SetActive(false);
                Player.transform.parent = Vehicle.transform;
                PlayerHealthBar.SetActive(false);
                playerCam.enabled = false;
                guiEnable = false;
                Vehicle.gameObject.GetComponent<CarUserControl>().enabled = true;
                inVehicle = true;
                CmdDisableModel();

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
