using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    private Inventory inventory;
    public int id;


    // Use this for initialization
    void Start () {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();// Talletetaan tieto esineestä,kun yritetään tiputtaa uuteen slottiin

        if (inventory.items[id].ID == -1)// tarkistaa sen slotin esineen id:n, mihin ollaan tiputtamassa esinettä. Jos ei ole niin voidaan sijoittaa esine sinne
        {
            Debug.Log(id);
            inventory.items[droppedItem.slotLocation] = new Item(); // nullaa esineen alkuperäisestä slotista, koska new Item() luo tyhjän esineen
            inventory.items[id] = droppedItem.item; // kerrotaan slotille mikä esine sinne laitettiin
            droppedItem.slotLocation = id;// päivittää itemille tiedon siitä missä slotissa se sijaitsee
        }
        else
        {
            // tämä kohta hoitaa sitä tapahtumaa, kun slotissa on esine. Esineet vaihtavat paikkaa
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slotLocation = droppedItem.slotLocation;
            item.transform.SetParent(inventory.slots[droppedItem.slotLocation].transform);
            item.transform.position = inventory.slots[droppedItem.slotLocation].transform.position;

            inventory.items[droppedItem.slotLocation] = item.GetComponent<ItemData>().item;
            inventory.items[id] = droppedItem.item;
            droppedItem.slotLocation = id;
           

        }
    }

}
