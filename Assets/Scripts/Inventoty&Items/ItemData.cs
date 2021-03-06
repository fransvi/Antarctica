﻿using System;
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
    public Canvas _cameraCanvas;
    public PlayerMovementScript equipAnimation;

    private Inventory inv;
    private Tooltip tooltip;
    private PlayerEquip equip;
    public ActionBar actionbar;
    private Slot slot;
    public bool equiped;
    public static int previousSlot;


    // Use this for initialization
    void Start()
    {
       
        Debug.Log("Tapahtuuvain kerran");
        //apu = GameObject.Find("Equipment").transform;
        equiped = false;
        inv = transform.root.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        tooltip = inv.GetComponent<Tooltip>();
        actionbar = inv.GetComponent<ActionBar>();

    }
    public void OnBeginDrag(PointerEventData eventData)//tapahtuu kun esineen siirto alkaa
    {
        if (item != null)//tsekkaus onko asia jota halutaan siirtää esine
        {
            //Debug.Log(eventData.pointerPress.gameObject.name);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position;//kerrotaan esineelle seurata kursoria
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            actionbar.Deactivate();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)//tapahtuu siirron aikana
    {
        if (item != null)//tsekkaus onko asia jota halutaan siirtää esine
        {
            

            this.transform.position = FollowMouse(_cameraCanvas);//kerrotaan esineelle seurata kursoria

        }

    }

    public void OnEndDrag(PointerEventData eventData)//tapahtuu siirron loputtua
    {
        Debug.Log(eventData.lastPress.gameObject.GetComponent<ItemData>().name);
        Debug.Log(eventData.lastPress.gameObject.GetComponent<ItemData>().equiped);
        this.transform.SetParent(inv.slots[slotLocation].transform);
        this.transform.position = inv.slots[slotLocation].transform.position;// Asetetaan esineen paikaksi halutun slotin paikka 
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        slot = transform.GetComponentInParent<Slot>();
        Debug.Log("edellinen slot " + previousSlot);
        Debug.Log("nykyinen slot " + slotLocation);
        Debug.Log(inv.slots[previousSlot].GetComponent<Slot>().equiped);



        if ((inv.slots[previousSlot].GetComponent<Slot>().equiped == true) && (eventData.lastPress.gameObject.GetComponent<ItemData>().equiped == true))
        {
            Debug.Log("Joooo");
            changeOutline(inv.slots[slotLocation].GetComponent<Slot>());
            if(previousSlot == slotLocation)
            {
                slot.GetComponent<Outline>().enabled = true;
                slot.equiped = true;
                equiped = true;
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
            slot = transform.GetComponentInParent<Slot>();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EquipItem(item, slot);

            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                actionbar.Activate(item, slot);
            }
        }
    }


    public Vector3 FollowMouse(Canvas cameraCanvas)//Auttaa esinettä seuraamaan hiirtä dragin aikana
    {
        _cameraCanvas = cameraCanvas;
        _cameraCanvas = transform.GetComponentInParent<Canvas>();
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = _cameraCanvas.planeDistance;
        Camera renderCamera = _cameraCanvas.worldCamera;
        Vector3 canvasPos = renderCamera.ScreenToWorldPoint(screenPos);

        return canvasPos;
    }

    public void EquipItem(Item item, Slot slot)
    {
        equipAnimation = transform.GetComponentInParent<PlayerMovementScript>();

        if ((slot.equiped == false && inv.slots[previousSlot].GetComponent<Slot>().equiped == false) && equiped == false)
        {
            //GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + item.Title), apu);
            //apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            equipAnimation.CheckItemAnimation(item.ID, equiped);
            changeOutline(slot);
            actionbar.equiped = true;
        }
        else if (slot.equiped == false && (inv.slots[previousSlot].GetComponent<Slot>().equiped == true))
        {

            //Destroy(apu.GetChild(0).gameObject);
            //GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + item.Title), apu);
            equipAnimation.CheckItemAnimation(item.ID, equiped);
            //apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            changeOutline(slot);
        }
        else if (slot.equiped == true && equiped == true)
        {
            //Destroy(apu.GetChild(0).gameObject);
            equipAnimation.CheckItemAnimation(item.ID, equiped);
            changeOutline(slot);
            actionbar.equiped = false;
        }
        //else
        //{

        //    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + item.Title), apu);
        //    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
        //    Debug.Log("eka kerta");
        //    previousSlot = slotLocation;
        //    changeOutline(slot);
        //}
        previousSlot = slotLocation;


    }


    public void changeOutline(Slot slot)
    {

        Debug.Log("edellinen slot " + previousSlot);
        Debug.Log("nykyinen slot " + slotLocation);
        Debug.Log(equiped);
        Debug.Log((previousSlot == slotLocation && slot.equiped == false));
        Debug.Log((previousSlot == slotLocation && slot.equiped == false) && equiped == false);
        Debug.Log((previousSlot != slotLocation && slot.equiped == false) && equiped == false);

        if ((previousSlot == slotLocation && slot.equiped == false) && equiped == false)
        {
            slot.GetComponent<Outline>().enabled = true;
            slot.equiped = true;
            equiped = true;
        }
        
        else if ((previousSlot == slotLocation && slot.equiped == true) && equiped == true)
        {
            slot.GetComponent<Outline>().enabled = false;
            slot.equiped = false;
            equiped = false;
        }
        else if ((previousSlot != slotLocation && slot.equiped == false) && equiped == false)
        {
            Debug.Log("ääääää");
            slot.GetComponent<Outline>().enabled = true;
            slot.GetComponent<Slot>().equiped = true;
            Debug.Log("asd " + inv.slots[previousSlot]);
            Debug.Log("asd " + inv.slots[previousSlot].GetComponent<Slot>().equiped);

            inv.slots[previousSlot].GetComponent<Slot>().equiped = false;

            if (inv.slots[previousSlot].transform.childCount != 0)
            {
                inv.slots[previousSlot].GetComponentInChildren<ItemData>().equiped = false;
            }
            
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;

            equiped = true;
            Debug.Log(inv.slots[previousSlot].GetComponent<Slot>().equiped);
        }
        else if ((previousSlot != slotLocation && slot.equiped == false) && equiped == true)
        {
            Debug.Log("öööööö");
            slot.GetComponent<Outline>().enabled = true;
            inv.slots[previousSlot].GetComponent<Outline>().enabled = false;

            inv.slots[previousSlot].GetComponent<Slot>().equiped = false;
            slot.GetComponent<Slot>().equiped = true;
        }

        Debug.Log("asfdadgSDGASFHESTDHSFGH");
        previousSlot = slotLocation;
        Debug.Log(previousSlot);
    }
}
