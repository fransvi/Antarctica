using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    public GameObject _light;
    public GameObject[] _lights;
    public bool _isActive;
    public bool _hasPower = true;
    public bool _isMainSwitch; 

	// Use this for initialization
	void Start () {
        if (!_isMainSwitch)
        {
            _light.SetActive(false);
        }
        else
        {
            foreach(GameObject g in _lights)
            {
                g.SetActive(false);
                
            }
        }
        _isActive = false;

    }

    public void Activate(bool b)
    {
        if (_hasPower)
        {


            if (!_isMainSwitch)
            {
                _light.SetActive(b);



            }
            else
            {
                foreach (GameObject g in _lights)
                {
                    g.SetActive(b);
                }
            }
            _isActive = b;
        }



    }

    private void Update()
    {
        if (!_hasPower)
        {
            if (_isMainSwitch)
            {
                foreach(GameObject g in _lights)
                {
                    g.SetActive(false);
                }
            }
            else
            {
                _light.SetActive(false);
            }
            _isActive = false;
        }

    }

}
