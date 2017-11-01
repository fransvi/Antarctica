using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour {

    public GameObject _hand;
    public Vector3 _north;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _hand.transform.rotation = Quaternion.LookRotation(_north);
    }
}
