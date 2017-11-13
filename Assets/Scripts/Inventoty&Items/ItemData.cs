using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Item item;

    public int id;
    public int amount;
    public int slotLocation;
    public Transform apu;

    
    //private Transform originalSlot;
    private Inventory inv;
    private Tooltip tooltip;
    private PlayerEquip equip;
    private ActionBar actionbar;
    private bool equiped;
    private static int previousSlot;


    // Use this for initialization
    void Start()
    {
        apu = GameObject.Find("Equipment").transform;
        previousSlot = 0;
        equiped = false;
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        tooltip = inv.GetComponent<Tooltip>();
        actionbar = inv.GetComponent<ActionBar>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)//tapahtuu kun esineen siirto alkaa
    {
        if (item != null)//tsekkaus onko asia jota halutaan siirtää esine
        {
            //Debug.Log("jaka"+ eventData.selectedObject.gameObject.GetComponent<ItemData>().id);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position;//kerrotaan esineelle seurata kursoria
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)//tapahtuu siirron aikana
    {
        if (item != null)//tsekkaus onko asia jota halutaan siirtää esine
        {
            this.transform.position = eventData.position;//kerrotaan esineelle seurata kursoria

        }

    }

    public void OnEndDrag(PointerEventData eventData)//tapahtuu siirron loputtua
    {
        
        this.transform.SetParent(inv.slots[slotLocation].transform);
        this.transform.position = inv.slots[slotLocation].transform.position;// Asetetaan esineen paikaksi halutun slotin paikka 
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (this.gameObject.GetComponent<ItemData>().id == apu.GetChild(0).GetComponent<ItemPick>().id)
        {
            Debug.Log("pöö");
            changeOutline(item);
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);//Aktivoi tooltipin kun hoveraa päällä

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();//Deaktivoi kun poistutaan
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        changeOutline(item);

        if (item != null)//tsekkaus onko asia jota halutaan käyttää esine 
        {
            Debug.Log("onesine");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //Transform apu = GameObject.Find("Equipment").transform;

                if (actionbar.equiped == false)
                {
                    
                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                    actionbar.equiped = true;
                }
                else if (item.ID != apu.GetChild(0).GetComponent<ItemPick>().id)
                {
                    Destroy(apu.GetChild(0).gameObject);
                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                }
                else if(actionbar.equiped == true)
                {

                    Destroy(apu.GetChild(0).gameObject);
                    actionbar.equiped = false;
                }

            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                actionbar.Activate(item);
            }
        }
    }

    public void changeOutline(Item item)
    {
        if(previousSlot == slotLocation && actionbar.equiped == false)
            inv.slots[slotLocation].GetComponent<Outline>().enabled = true;

        if(previousSlot == slotLocation && actionbar.equiped == true)
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;

        if (previousSlot != slotLocation && actionbar.equiped == false)
        {
            inv.slots[slotLocation].GetComponent<Outline>().enabled = true;
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;
        }
        if (previousSlot != slotLocation && actionbar.equiped == true)
        {
            inv.slots[slotLocation].GetComponent<Outline>().enabled = true;
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;
        }

        previousSlot = slotLocation;

    }
}
