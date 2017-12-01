using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AntennaSpot : MonoBehaviour {

    private NetworkInstanceId radioId;

    private void Start()
    {
        radioId = transform.parent.GetComponent<NetworkIdentity>().netId;
    }

	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Antenna(Clone)")
        {
            other.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, true, other.transform.root.GetComponent<NetworkIdentity>().netId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Antenna(Clone)")
        {
            other.transform.root.GetComponent<NetworkRadio>().AntennaInPlace(radioId, false, other.transform.root.GetComponent<NetworkIdentity>().netId);
        }
    }
}
