using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

    private GameObject actionbar;
    private Item _item;
    public bool equiped;
    private ItemData data;
    private Inventory inv;

    public Button euipButton;
    public Button dropButton;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        actionbar = GameObject.Find("ActionBar");
        actionbar.SetActive(false);
        equiped = false;

        Button btn = euipButton.GetComponent<Button>();
        Button btn2 = dropButton.GetComponent<Button>();
        btn.onClick.AddListener(() => { EquipItem(_item);});
        btn2.onClick.AddListener(() => { DropItem(_item);});
    }

    public void Activate (Item item) {

        _item = item;
        actionbar.SetActive(true);
        actionbar.transform.position = Input.mousePosition;

    }

	public void Deactivate () {

        actionbar.SetActive(false);

    }
    void EquipItem(Item item)
    {
        _item = item;
        Transform apu = GameObject.Find("Equipment").transform;
        if (equiped == false)
        {
            Debug.Log("asdasd");
            GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
            Debug.Log(_item.Title);
            apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            equiped = true;
        }
        else if (equiped == true)
        {
            Destroy(apu.GetChild(0).gameObject);
            equiped = false;
        }
    }

    void DropItem(Item item)
    {
        Transform apu = GameObject.Find("Equipment").transform;

        Destroy(apu.GetChild(0).gameObject);
        equiped = false;
        inv.RemoveItem(item.ID);
        actionbar.SetActive(false);

        GameObject itemtodrop = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), new Vector3(0, 0, 1) * Time.deltaTime, Quaternion.identity);
    }
}
