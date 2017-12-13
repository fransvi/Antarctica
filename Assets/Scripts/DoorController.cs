using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Animator _animator = null;
    public bool _isOpen = false;
    public bool _requiresKey;
    private bool _guiEnable = false;
    public bool _requiresPower;
    public bool _hasPower;
    private string _doorLockedString = "The door is locked.";
    private string _doorRequiresPower = "The door requires power to open.";
    private string _doorRequiresKeyCode = "This door requires a keycode to open.";
    public string _doorInfo;
    public bool _requiresKeyCode;




    void Start () {
        _animator = GetComponent<Animator>();

    }
    IEnumerator ShowText()
    {
        _guiEnable = true;
        yield return new WaitForSeconds(3f);
        _guiEnable = false;
    }

    public void OpenDoor(bool hasKey, bool hasKeyCode)
    {
        if(_requiresPower && _requiresKeyCode)
        {
            if (_hasPower)
            {
                if (hasKeyCode)
                {
                    gameObject.GetComponent<AudioSource>().Play();
                    _animator.SetTrigger("OpenDoor");
                }
                else
                {
                    _doorInfo = _doorRequiresKeyCode;
                    StartCoroutine(ShowText());
                }

            }
            else
            {
                _doorInfo = _doorRequiresPower;
                StartCoroutine(ShowText());
            }
        }else if(_requiresKey && !_requiresPower && !_requiresKeyCode)
        {
            if (hasKey)
            {
                //gameObject.GetComponent<AudioSource>().Play();
                _animator.SetTrigger("OpenDoor");
            }
            else
            {
                _doorInfo = _doorLockedString;
                StartCoroutine(ShowText());
            }
        }else if(!_requiresKey && !_requiresKeyCode && !_requiresPower)
        {
            //gameObject.GetComponent<AudioSource>().Play();
            _animator.SetTrigger("OpenDoor");
        }
        
    }
    public void CloseDoor()
    {
        if (!_requiresKey)
        {
            gameObject.GetComponent<AudioSource>().Play();
            _animator.enabled = true;
            _isOpen = false;
        }
 

    }

    public void PauseAnim()
    {
        _isOpen = true;
        _animator.enabled = false;
    }

    //public void OnGUI()
    //{
    //    if (_guiEnable != false)
    //    {
    //        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), _doorInfo);
    //    }
    //    else
    //    {
    //        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
    //    }
    //}
}
