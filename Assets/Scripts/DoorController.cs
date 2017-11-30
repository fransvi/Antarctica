using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Animator _animator = null;
    public bool _isOpen = false;


	void Start () {
        _animator = GetComponent<Animator>();

    }
 
    public void OpenDoor()
    {
        _animator.SetTrigger("OpenDoor");
    }
    public void CloseDoor()
    {
        _animator.enabled = true;
        _isOpen = false;
    }

    public void PauseAnim()
    {
        _isOpen = true;
        _animator.enabled = false;
    }
}
