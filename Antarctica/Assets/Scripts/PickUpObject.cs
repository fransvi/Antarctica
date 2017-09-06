using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour {

	public Transform camera;
	RaycastHit raycastHit;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay(camera.position, camera.TransformDirection (Vector3.forward), Color.green);
		if (Physics.Raycast (camera.position, camera.TransformDirection (Vector3.forward), out raycastHit, 5.0f)) {
			if (raycastHit.transform.gameObject!=gameObject){ // Tähän tarkistus, onko objekti poimittava (tällä hetkellä tarkistaa vain ettei objekti ole pelaaja itse)
				if (Input.GetMouseButtonUp(0)) 
				{
					// Tänne koodi objektin lisäämiseksi inventaariin (tällä hetkellä vain tuhoaa objektin)
					Destroy(raycastHit.transform.gameObject);
				}
			}
		}
	}
}