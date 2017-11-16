using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {


    public float _upForce;
	// Use this for initialization
	void Start () {
		
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _upForce);

       
		
	}
}
