using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

    public GameObject actionbar;
    private Item _item;
    public bool equiped;
    private ItemData data;
    private Inventory inv;
    private PlayerHealth health;
    public Canvas cameraCanvas;

    private Slot _slot;
    public Button equipButton;
    public Button dropButton;
    public Button consumeButton;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        //actionbar = GameObject.Find("ActionBar");
        equiped = false;


        Button btn = equipButton.GetComponent<Button>();
        Button btn2 = dropButton.GetComponent<Button>();
        Button btn3 = consumeButton.GetComponent<Button>();
        btn.onClick.AddListener(() => { EquipItem(_item, _slot); });
        btn2.onClick.AddListener(() => { DropItem(_item); });
        btn3.onClick.AddListener(() => { ConsumeItem(_item); });
        actionbar.SetActive(false);
        //data.equiped = false;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){

            this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyReduceHealth(20);
        }
    }

    public void Activate (Item item, Slot slot)
    {

        _item = item;
        _slot = slot;
        actionbar.SetActive(true);

        Vector3 screenPos = Input.mousePosition;
        screenPos.z = cameraCanvas.planeDistance;
        Camera renderCamera = cameraCanvas.worldCamera;
        Vector3 canvasPos = renderCamera.ScreenToWorldPoint(screenPos);
        actionbar.transform.position = canvasPos;

    }

	public void Deactivate ()
    {

        actionbar.SetActive(false);

    }
    public void EquipItem(Item item, Slot slot)
    {
        _item = item;
        _slot = slot;

        //data.EquipItem(item, slot, equiped);
        Debug.Log("asdasd" + slot.id);
        Transform apu = GameObject.Find("Equipment").transform;

        data = slot.GetComponentInChildren<ItemData>();
        data.slotLocation = slot.id;
        if (equiped == false)
        {
            
            GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
            apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            data.changeOutline(slot);
            equiped = true;
            Debug.Log("actionar equiped " + equiped);
        }
        else if (item.ID != apu.GetChild(0).GetComponent<ItemPick>().id)
        {
            Destroy(apu.GetChild(0).gameObject);
            Debug.Log("gg"); 
            GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
            apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            data.changeOutline(slot);
        }
        else if (equiped == true)
        {
            Destroy(apu.GetChild(0).gameObject);
            data.changeOutline(slot);
            equiped = false;
        }
        ItemData.previousSlot = data.slotLocation;
    }

    void ConsumeItem(Item item)
    {

        this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseHunger(item.Healthamount);
        this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseHealth(item.Healthamount);
        Debug.Log("Heatlh increased" + item.Healthamount + " and Hunger increased" + item.Healthamount);


        inv.RemoveItem(item.ID);
        actionbar.SetActive(false);

    }

    void DropItem(Item item)
    {
        Transform apu = GameObject.Find("Equipment").transform;

        if (equiped == true)
        {
            Destroy(apu.GetChild(0).gameObject);
        }

        equiped = false;
        inv.RemoveItem(item.ID);
        actionbar.SetActive(false);

        GameObject itemtodrop = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), this.transform.position + Vector3.forward, Quaternion.identity);
    }


}
