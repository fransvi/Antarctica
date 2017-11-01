﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Item item;

    public int id;
    public int amount;
    public int slotLocation;
     
    //private Transform originalSlot;
    private Inventory inv;
    private Tooltip tooltip;
    private PlayerEquip equip;
    private ActionBar actionbar;
    private bool equiped;


    // Use this for initialization
    void Start()
    {
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
            Debug.Log("onesine");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Transform apu = GameObject.Find("Equipment").transform;
                if (actionbar.equiped == false)
                {
                    GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + eventData.pointerPress.name), apu);
                    apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
                    actionbar.equiped = true;
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
}
