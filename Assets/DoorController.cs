using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Animator _animator = null;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
    public void OpenDoor()
    {
        _animator.SetBool("isOpen", true);
    }
    public void CloseDoor()
    {
        _animator.SetBool("isOpen", false);
    }
}
