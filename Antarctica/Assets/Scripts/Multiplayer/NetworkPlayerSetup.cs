using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerSetup : NetworkBehaviour
{

    public Behaviour[] componentsToEnable;
    public Camera playerCamera;
    public GameObject playerUi;



    public override void OnStartLocalPlayer()
    {

        gameObject.layer = LayerMask.NameToLayer("LocalPlayer");

        //Enable Components
        foreach (Behaviour component in componentsToEnable)
        {
            component.enabled = true;
        }

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


}
