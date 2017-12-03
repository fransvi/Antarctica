using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneratorScript : MonoBehaviour {


    public bool _powerOn = false;
    public GameObject _powerLight;
    public GameObject _powerLight2;
    private Animator _animator = null;
    public GameObject _housingOutdoorLight;
    public GameObject _researchOutdoorLight;
    public GameObject[] _housingSwitches;
    public GameObject _researchSwitch;
    public GameObject[] _researchDoors;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _powerLight.GetComponent<Renderer>().material.color = Color.red;
        _housingOutdoorLight.SetActive(true);
        foreach(GameObject g in _housingSwitches)
        {
            g.GetComponent<LightSwitch>()._hasPower = true;
        }
        foreach (GameObject g in _researchDoors)
        {
            g.GetComponent<DoorController>()._hasPower = false;
        }
        _powerLight2.GetComponent<Renderer>().material.color = Color.green;
        _researchOutdoorLight.SetActive(false);
        _researchSwitch.GetComponent<LightSwitch>()._hasPower = false;
    }
	

    public void EnablePower()
    {
        _animator.SetTrigger("SwitchUp");
    }
    public void DisablePower()
    {
        _animator.enabled = true;
        _powerOn = false;
        _powerLight.GetComponent<Renderer>().material.color = Color.red;
        _housingOutdoorLight.SetActive(true);
        foreach (GameObject g in _housingSwitches)
        {

            g.GetComponent<LightSwitch>()._hasPower = true;
        }
        foreach (GameObject g in _researchDoors)
        {
            g.GetComponent<DoorController>()._hasPower = false;
        }
        _powerLight2.GetComponent<Renderer>().material.color = Color.green;
        _researchOutdoorLight.SetActive(false);
        _researchSwitch.GetComponent<LightSwitch>()._hasPower = false;
    }

    public void PauseAnim()
    {
        _powerLight.GetComponent<Renderer>().material.color = Color.green;
        _researchOutdoorLight.SetActive(true);
        _powerLight2.GetComponent<Renderer>().material.color = Color.red;
        _housingOutdoorLight.SetActive(false);
        _researchSwitch.GetComponent<LightSwitch>()._hasPower = true;
        foreach (GameObject g in _housingSwitches)
        {

            g.GetComponent<LightSwitch>()._hasPower = false;
        }
        foreach (GameObject g in _researchDoors)
        {
            g.GetComponent<DoorController>()._hasPower = true;
        }
        _powerOn = true;
        _animator.enabled = false;
    }
}
