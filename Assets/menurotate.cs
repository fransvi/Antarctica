using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menurotate : MonoBehaviour {

    public float speed = 2f;
    public float maxRotation = 45f;


    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, maxRotation * Mathf.Sin(Time.time * speed), 0f);
    }
}
