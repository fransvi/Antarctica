using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Item _item;
    private string data;
    public GameObject tooltip;
    public Canvas _canvas;

    void Start()
    {
        //tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    void Update()
    {

        if (tooltip.activeSelf)
        {

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out pos);
            transform.position = _canvas.transform.TransformPoint(pos);

            tooltip.transform.localPosition = Input.mousePosition - new Vector3(470, 137, 0);//Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
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