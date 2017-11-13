using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FindObjectOfType<TerrainScript>().GetPlayer(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
