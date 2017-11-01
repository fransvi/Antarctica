using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {

    public GameObject[] _currentItems;
    public GameObject _lantern;
    public GameObject _compass;
    private bool _lanternActive = false;
    private bool _compassActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!_lanternActive && Input.GetKeyUp(KeyCode.T))
        {
            _lantern.SetActive(true);
            _lantern.GetComponent<LanternScript>()._isActive = true;
            _lanternActive = true;
        }
        else if(_lanternActive && Input.GetKeyUp(KeyCode.T))
        {
            _lantern.SetActive(false);
            _lantern.GetComponent<LanternScript>()._isActive = false;
            _lanternActive = false;
        }
        if(!_compassActive && Input.GetKeyUp(KeyCode.Y))
        {
            _compass.SetActive(true);
            //_compass.GetComponent<CompassScript>().isActive = true;
            _compassActive = true;
        }else if(_compassActive && Input.GetKeyUp(KeyCode.Y))
        {
            _compass.SetActive(false);
            //_compass.GetComponent<CompassScript>()._isActive = false;
            _compassActive = false;
        }
		
	}
}
