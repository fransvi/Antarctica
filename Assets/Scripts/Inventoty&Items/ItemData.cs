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
    private Slot slot;
    public bool equiped;
    public static int previousSlot;


    // Use this for initialization
    void Start()
    {
        apu = GameObject.Find("Equipment").transform;
        
        
        //previousSlot = 0;
        //equiped = false;
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
        slot = transform.GetComponentInParent<Slot>();
        Debug.Log("edellinen slot " + previousSlot);
        Debug.Log("nykyinen slot " + slotLocation);
        Debug.Log("pinacolada" + inv.slots[previousSlot].GetComponent<Slot>().equiped + " " + inv.slots[slotLocation].GetComponent<Slot>().equiped);

        if ((inv.slots[previousSlot].GetComponent<Slot>().equiped == true) && (apu.GetChild(0).GetComponent<ItemPick>().id == gameObject.GetComponent<ItemData>().id))
        {
            Debug.Log("kaljaa");
            changeOutline(inv.slots[slotLocation].GetComponent<Slot>());
            if(previousSlot == slotLocation)
            {
                Debug.Log("Sagriaa");
                slot.GetComponent<Outline>().enabled = true;
                slot.equiped = true;
                actionbar.equiped = true;
            }
                
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
        
        if (item != null)//tsekkaus onko asia jota halutaan käyttää esine 
        {
            //Debug.Log(inv.slots[previousSlot].GetComponent<Slot>().equiped);
            Debug.Log("onesine");
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                slot = transform.GetComponentInParent<Slot>();

                Debug.Log("Viski " + slot.id);
                Debug.Log(actionbar.equiped + " OLUTTA");
                if ((slot.equiped == false && inv.slots[previousSlot].GetComponent<Slot>().equiped == false) && equiped == false)
                {
                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                    Debug.Log("asdasadasd");
                    changeOutline(slot);
                }
                else if (slot.equiped == false && (inv.slots[previousSlot].GetComponent<Slot>().equiped == true))
                {
                    Debug.Log("JAllukola");
                    //slot.equiped = true;

                    Destroy(apu.GetChild(0).gameObject);
                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                    changeOutline(slot);
                }
                else if(slot.equiped == true && actionbar.equiped == true)
                {
                    Destroy(apu.GetChild(0).gameObject);
                    //equiped = false;
                    //slot.equiped = false;
                    changeOutline(slot);
                }
                else
                {

                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                    Debug.Log("eka kerta");
                    previousSlot = slotLocation;
                    changeOutline(slot);
                }
                previousSlot = slotLocation;
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                actionbar.Activate(item);
            }
        }
    }

    public void changeOutline(Slot slot)
    {
        Debug.Log("edellinen slot " + previousSlot);
        Debug.Log("nykyinen slot " + slotLocation);
        Debug.Log(slot.equiped);
        Debug.Log((previousSlot == slotLocation && slot.equiped == false));


        if ((previousSlot == slotLocation && slot.equiped == false) && actionbar.equiped == false)
        {
            slot.GetComponent<Outline>().enabled = true;
            slot.equiped = true;
            actionbar.equiped = true;
        }
        
        else if ((previousSlot == slotLocation && slot.equiped == true) && actionbar.equiped == true)
        {
            Debug.Log("ei pitäisi käudä");
            slot.GetComponent<Outline>().enabled = false;
            slot.equiped = false;
            actionbar.equiped = false;
        }
        else if ((previousSlot != slotLocation && slot.equiped == false) && actionbar.equiped == false)
        {
            Debug.Log("ääääää");
            slot.GetComponent<Outline>().enabled = true;
            slot.GetComponent<Slot>().equiped = true;
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;
            inv.slots[previousSlot].GetComponent<Slot>().equiped = false;
            actionbar.equiped = true;
        }
        else if ((previousSlot != slotLocation && slot.equiped == false) && actionbar.equiped == true)
        {
            Debug.Log("öööööö");
            slot.GetComponent<Outline>().enabled = true;
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;

            inv.slots[previousSlot].GetComponent<Slot>().equiped = false;
            slot.GetComponent<Slot>().equiped = true;
        }

        previousSlot = slotLocation;

    }
}
