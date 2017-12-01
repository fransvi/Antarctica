using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Animator _animator = null;
    public bool _isOpen = false;
    public bool _isLocked;
    private bool _guiEnable = false;


	void Start () {
        _animator = GetComponent<Animator>();

    }
    IEnumerator ShowText()
    {
        _guiEnable = true;
        yield return new WaitForSeconds(3f);
        _guiEnable = false;
    }

    public void OpenDoor()
    {
        if (!_isLocked)
        {
            _animator.SetTrigger("OpenDoor");
        }
        else
        {
            StartCoroutine(ShowText());
        }
    }
    public void CloseDoor()
    {
        if (!_isLocked)
        {
            _animator.enabled = true;
            _isOpen = false;
        }
 

    }

    public void PauseAnim()
    {
        _isOpen = true;
        _animator.enabled = false;
    }

    void OnGUI()
    {
        if (_guiEnable != false)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), "The door is locked.");
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
        }
    }
}
