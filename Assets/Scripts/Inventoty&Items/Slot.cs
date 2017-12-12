using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    private Inventory inventory;
    private ItemData data;
    public int id;
    public bool equiped;


    // Use this for initialization
    void Start () {

        inventory = transform.root.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        equiped = false;

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
            Debug.Log("asdasdasdasfsfdbadfb" + item.GetComponent<ItemData>().slotLocation);
            Debug.Log("asdasdasdasfsfdbadfb" + item.GetComponent<ItemData>().item.Title);
            Debug.Log("asdasdasdasfsfdbadfb" + droppedItem.name);


            Debug.Log(inventory.slots[droppedItem.slotLocation].GetComponent<Slot>().equiped);
            Debug.Log(inventory.slots[ItemData.previousSlot].GetComponent<Slot>().equiped);
            Debug.Log(droppedItem.slotLocation != ItemData.previousSlot);

            if (inventory.slots[droppedItem.slotLocation].GetComponent<Slot>().equiped == true || inventory.slots[ItemData.previousSlot].GetComponent<Slot>().equiped == true)
            {
                if (droppedItem.slotLocation != ItemData.previousSlot)
                {
                    Debug.Log(inventory.slots[droppedItem.slotLocation].GetComponent<Slot>().id);
                    Debug.Log("Bissee");
                    item.GetComponent<ItemData>().changeOutline(inventory.slots[droppedItem.slotLocation].GetComponent<Slot>());
                }
            }

            item.transform.SetParent(inventory.slots[droppedItem.slotLocation].transform);
            item.transform.position = inventory.slots[droppedItem.slotLocation].transform.position;
            


            inventory.items[droppedItem.slotLocation] = item.GetComponent<ItemData>().item;
            inventory.items[id] = droppedItem.item;
            droppedItem.slotLocation = id;
           

        }
    }

}
