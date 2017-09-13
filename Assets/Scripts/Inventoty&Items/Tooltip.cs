using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Item _item;
    private string data;
    private GameObject tooltip;

    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    void Update()
    {

        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
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