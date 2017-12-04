using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ActionBar : NetworkBehaviour {

    public GameObject actionbar;
    private Item _item;
    public bool equiped;
    private ItemData data;
    private Inventory inv;
    private PlayerHealth health;
    public Canvas cameraCanvas;
    public PlayerMovementScript equipAnimation;

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
        btn3.onClick.AddListener(() => { ConsumeItem(_item, _slot); });
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
        Transform apu = GameObject.Find("Equipment").transform;

        data = slot.GetComponentInChildren<ItemData>();
        data.slotLocation = slot.id;
        if (equiped == false)
        {
            
            //GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
            //apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            equipAnimation.CheckItemAnimation(item.ID, data.equiped);
            data.changeOutline(slot);
            equiped = true;
            Debug.Log("actionar equiped " + equiped);
        }
        else if (equiped == true && (inv.slots[ItemData.previousSlot].GetComponent<Slot>().equiped == true))
        {
            //Destroy(apu.GetChild(0).gameObject);
            //GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
           
            //apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            equipAnimation.CheckItemAnimation(item.ID, data.equiped);
            data.changeOutline(slot);
            
        }
        else if (equiped == true)
        {
            //Destroy(apu.GetChild(0).gameObject);
            equipAnimation.CheckItemAnimation(item.ID, data.equiped);
            data.changeOutline(slot);
            equiped = false;
        }
        ItemData.previousSlot = data.slotLocation;
    }

    void ConsumeItem(Item item, Slot slot)
    {
        if (equiped == true)
        {
            data = slot.GetComponentInChildren<ItemData>();
            data.slotLocation = slot.id;
            this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseHunger(item.Healthamount);
            this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseHealth(item.Healthamount);
            this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseThirst(item.Thirstamount);
            Debug.Log("Heatlh increased" + item.Healthamount + " and Hunger increased" + item.Healthamount);
            equipAnimation.CheckItemAnimation(item.ID, equiped);
            data.changeOutline(slot);

            inv.RemoveItem(item.ID);
            actionbar.SetActive(false);
            equiped = false;
        }

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

        Object itemtodrop = Network.Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), this.transform.position + Vector3.forward, Quaternion.identity,0);
    }


}
