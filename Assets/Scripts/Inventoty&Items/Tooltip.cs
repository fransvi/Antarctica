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

            //var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            //screenPoint.z = 10.0f; //distance of the plane from the camera
            //tooltip.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = cameraCanvas.planeDistance;
            Camera renderCamera = cameraCanvas.worldCamera;
            Vector3 canvasPos = renderCamera.ScreenToWorldPoint(screenPos);
            tooltip.transform.position = canvasPos;

            //tooltip.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
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