using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

    public GameObject playerPointer;
    public GameObject player;

	// Use this for initialization
	void Start () {
        player = this.transform.parent.parent.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        playerPointer.transform.localPosition = new Vector3(player.transform.position.x / 500, 0.3f, (player.transform.position.z / 500));
		
	}
}
