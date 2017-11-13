using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    public float ropePartDistance;

	// Use this for initialization
	void Start () {

        GetComponent<Rigidbody>().sleepThreshold = 5f;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform ropePart = transform.GetChild(i);
            ropePart.localPosition = new Vector3(0f, -ropePartDistance * i, 0f);
            Rigidbody ropePartRigid = ropePart.GetComponent<Rigidbody>();

            if (ropePart.name != "Head" && gameObject.name == "RopeCharacterJoint")
                ropePart.GetComponent<CharacterJoint>().connectedBody = transform.GetChild(i - 1).GetComponent<Rigidbody>();

            ropePartRigid.sleepThreshold = 5f;
        }
	}
}
