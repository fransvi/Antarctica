using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour {

    public Light _light;
    public float _intensityTickAmount;
    public float _intensityTickTimer;
    public bool _isActive;
    public float _fuelAmount;
    public float _fuelReductionTick;
    private bool _reducingFuel = true;
    private bool _reducingLight = true;
    private bool _flicker = true;
	// Use this for initialization
	void Start () {

        _light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_isActive && _reducingFuel)
        {
            _reducingFuel = false;
            StartCoroutine(ReduceFuel());
            if(_fuelAmount < 50f && _reducingLight)
            {
                _reducingLight = false;
                StartCoroutine(ReduceLight());
            }
            if (_flicker)
            {
                _flicker = false;
                StartCoroutine(LampFlicker());
            }


        }
	}

    public void RefillFuel()
    {
        _fuelAmount = 100f;
        _light.intensity = 1f;
    }

    IEnumerator LampFlicker()
    {
        _light.intensity -= 0.1f;
        yield return new WaitForSeconds(0.5f);
        _light.intensity += 0.1f;
        _flicker = true;
    }

    IEnumerator ReduceLight()
    {
        yield return new WaitForSeconds(_intensityTickTimer);
        _light.intensity -= _intensityTickAmount;
        _reducingLight = true;
    }
    IEnumerator ReduceFuel()
    {
        _fuelAmount -= _fuelReductionTick;
        yield return new WaitForSeconds(5f);
        _reducingFuel = true;
    }
}
