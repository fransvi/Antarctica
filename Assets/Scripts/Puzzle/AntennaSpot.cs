using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AntennaSpot : MonoBehaviour {

    private NetworkInstanceId radioId;
    private GameObject player;
    private bool antennaInHand = false;
    private bool trueSent = false;
    private bool falseSent = false;

    private void Start()
    {
        radioId = transform.parent.GetComponent<NetworkIdentity>().netId;
    }

    private void FixedUpdate()
    {
        if(player != null)
        {
            if(player.GetComponent<PlayerMovementScript>().itemHeld == "Antenna" && trueSent == false)
            {
                player.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, true, player.transform.root.GetComponent<NetworkIdentity>().netId);
                trueSent = true;
                falseSent = false;
            }
            else if(player.GetComponent<PlayerMovementScript>().itemHeld != "Antenna" && falseSent == false)
            {
                player.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, false, player.transform.root.GetComponent<NetworkIdentity>().netId);
                falseSent = true;
                trueSent = false;
            }
        }
    }

	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.gameObject.layer == 8)
        {
            player = other.gameObject;

            if (other.gameObject.GetComponent<PlayerMovementScript>().itemHeld == "Antenna")
            {
                other.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, true, other.transform.root.GetComponent<NetworkIdentity>().netId);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            if (player.GetComponent<PlayerMovementScript>().itemHeld == "Antenna")
            {
                other.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, false, other.transform.root.GetComponent<NetworkIdentity>().netId);
            }
            trueSent = false;
            falseSent = false;
            player = null;
        }
    }
}
