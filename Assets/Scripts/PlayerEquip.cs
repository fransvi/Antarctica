using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

    public GameObject _lantern;
    private bool _active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!_active && Input.GetKeyUp(KeyCode.T))
        {
            _lantern.SetActive(true);
            _lantern.GetComponent<LanternScript>()._isActive = true;
            _active = true;
        }
        else if(_active && Input.GetKeyUp(KeyCode.T))
        {
            _lantern.SetActive(false);
            _lantern.GetComponent<LanternScript>()._isActive = false;
            _active = false;
        }
		
	}
}
