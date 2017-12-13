using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour {
    private bool _flickering = false;
    private bool _textActive = false;
    private bool _guiEnable;
    //private Image _Textpanel;
    public string _itemText;

    // Use this for initialization
    void Start () {
        _guiEnable = false;
	}

    //public void ShowText(bool b, Image Texpanel)
    //{
    //    _guiEnable = b;
    //    _Textpanel = Texpanel;
    //    Debug.Log("adsas");
    //}

    //void OnGUI ()
    //{
    //    if (_guiEnable != false)
    //    {
    //        Debug.Log("sasasas");
    //        _Textpanel.gameObject.SetActive(true);
    //        _Textpanel.gameObject.GetComponentInChildren<Text>().text = _itemText;
    //        //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 640, 480), _itemText);
    //    }
    //    else if(_guiEnable == false)
    //    {
    //        _Textpanel.gameObject.SetActive(true);
    //        _Textpanel.gameObject.GetComponentInChildren<Text>().text = " ";
    //        //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
    //    }
    //}

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!_flickering)
        {
            StartCoroutine(Flicker());
        }

    }
    IEnumerator Flicker()
    {
        _flickering = true;
        //Debug.Log(gameObject.GetComponentInChildren<Renderer>().material.color);
        //execute code here.
        for (int i = 0; i < 10; ++i)
        {
            float r = gameObject.GetComponentInChildren<Renderer>().material.color.r + 0.035f;
            float b = gameObject.GetComponentInChildren<Renderer>().material.color.b + 0.035f;
            float g = gameObject.GetComponentInChildren<Renderer>().material.color.g + 0.035f;
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(r, g, b));
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 10; ++i)
        {
            float r = gameObject.GetComponentInChildren<Renderer>().material.color.r - 0.035f;
            float b = gameObject.GetComponentInChildren<Renderer>().material.color.b - 0.035f;
            float g = gameObject.GetComponentInChildren<Renderer>().material.color.g - 0.035f;
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(r, g, b));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        _flickering = false;

    }
}
