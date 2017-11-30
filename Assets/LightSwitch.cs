using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    public GameObject _light;
    public bool _isActive;
    public bool _hasPower = true;

	// Use this for initialization
	void Start () {
        _light.SetActive(false);
        _isActive = false;
	}

    public void Activate(bool b)
    {

          _light.SetActive(b);
          _isActive = b;
        

    }

    private void Update()
    {

    }

}
