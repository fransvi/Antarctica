using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Item _item;
    private string data;
    public GameObject tooltip;
    public Canvas cameraCanvas;

    void Start()
    {
        //tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    void Update()
    {

        if (tooltip.activeSelf)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = cameraCanvas.planeDistance;
            Camera renderCamera = cameraCanvas.worldCamera;
            Vector3 canvasPos = renderCamera.ScreenToWorldPoint(screenPos);
            tooltip.transform.position = canvasPos;
        }
    }


    public void Activate(Item item)
    {
        _item = item;
        ConstructDataString();
        tooltip.SetActive(true);

    }
	public void Deactivate()
    {
        tooltip.SetActive(false);
    }
    public void ConstructDataString()
    {
        Debug.Log(_item.Description);
        data = "<color=#F6FF94FF><b>" + _item.Title + "</b></color>\n\n" + _item.Description + "";
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data; 
    }
}