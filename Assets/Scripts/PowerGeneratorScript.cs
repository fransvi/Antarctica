using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneratorScript : MonoBehaviour {


    public bool _powerOn = false;
    public GameObject _powerLight;
    public GameObject _powerLight2;
    private Animator _animator = null;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _powerLight.GetComponent<Renderer>().material.color = Color.red;
        _powerLight2.GetComponent<Renderer>().material.color = Color.green;
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
        _powerLight2.GetComponent<Renderer>().material.color = Color.green;
    }

    public void PauseAnim()
    {
        _powerLight.GetComponent<Renderer>().material.color = Color.green;
        _powerLight2.GetComponent<Renderer>().material.color = Color.red;
        _powerOn = true;
        _animator.enabled = false;
    }
}
