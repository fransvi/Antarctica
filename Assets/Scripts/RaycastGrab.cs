using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGrab : MonoBehaviour {
    private RaycastHit vision;
    public float rayLength;
    private bool isGrabbed;
    private Rigidbody grabbedObject;
    private Camera playerCamera;

	// Use this for initialization
	void Start () {
        rayLength = 4.0f;
        isGrabbed = false;
        playerCamera = GetComponentInParent<Camera>();

		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayLength, Color.red, 0.5f);

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out vision, rayLength))
        {
            if(vision.collider.tag == "Interactive")
            {
                Debug.Log(vision.collider.name);
                if(Input.GetKeyDown(KeyCode.E) && !isGrabbed)
                {
                    Debug.Log("Grab:");
                    grabbedObject = vision.rigidbody;
                    grabbedObject.isKinematic = true;
                    grabbedObject.transform.SetParent(gameObject.transform);
                    isGrabbed = true;
                }
                else if(isGrabbed && Input.GetKeyDown(KeyCode.E))
                {
                    grabbedObject.transform.parent = null;
                    grabbedObject.isKinematic = false;
                    isGrabbed = false;
                }
            }
        }

	}
}
