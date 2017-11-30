using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneratorScript : MonoBehaviour {


    public bool _powerOn;
    public GameObject _powerLight;
    private Animator _animator = null;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _powerLight.GetComponent<Renderer>().material.color = Color.red;
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
    }

    public void PauseAnim()
    {
        _powerLight.GetComponent<Renderer>().material.color = Color.green;
        _powerOn = true;
        _animator.enabled = false;
    }
}
