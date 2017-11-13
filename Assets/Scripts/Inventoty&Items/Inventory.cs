using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{

    //referenssit kyseisiin objekteihin
    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        database = GetComponent<ItemDatabase>();

        slotAmount = 20;
        inventoryPanel = GameObject.Find("InventoryPanel");//Etsii tallentaa kyseiset gameobjektit muuttujiin
        slotPanel = GameObject.Find("SlotPanel");

        for (int i = 0; i < 20; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));//luodaan uusia slotteja listaan
            slots[i].GetComponent<Slot>().id = i; // asetetaan kyseisen slotin id-muuttujaan id-numero
            slots[i].transform.SetParent(slotPanel.transform);//asetetaan uudet slotit olemaan slotPanelin lapsia
        }

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            RemoveItem(2);
        }
    }

    //Lisää esineprefabin ja asettaa arvot annetun id perusteella
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemById(id);// täyttää kyseisen esineobjektin sillä kyseisellä id:llä olevan esineen databasesta
        if (items.Contains(itemToAdd) && itemToAdd.Stackable)//Tarkistetaan onko kyseinen esine jo inventoryssa ja onko se stackable
        {
            for (int i = 0; i < items.Count; i++)
            {
                //Ainakin toistaiseksi lisätään vain tekstikenttään itemeiden määrä
                if (items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }

            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)//tsekkaa onko esine olemsassa
                {
                    items[i] = itemToAdd;//vie kyseisen inventoryn paikkaan haetun esineen
                    GameObject itemObj = Instantiate(inventoryItem); //luo fyysisen kopion esineprefabista
                    itemObj.GetComponent<ItemData>().item = itemToAdd; //Asettaa tiedon itemdata-luokan item muuttujalle siitä mikä esine luotiin
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.GetComponent<ItemData>().id = id;
                    itemObj.GetComponent<ItemData>().slotLocation = i; //päivitetään tieto siitä missä slotissa esine on slotin oman järjestys id:n mukaan
                    itemObj.transform.SetParent(slots[i].transform); //Asettaa esineen olemaan slotin lapsi
                    itemObj.transform.position = slots[i].transform.position;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;// Asettaa prefabin source imagen olemaan esineen nimen mukainen Sprite
                    itemObj.name = itemToAdd.Title;// Asettaa slotissa olevien esineiden nimeksi databasessa olevan nimen. Tämä näkyy inspectorissa

                    break;
                }

            }
        }
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = database.FetchItemById(id);
        if (itemToRemove.Stackable && items.Contains(itemToRemove))
        {
            for (int j = 0; j < items.Count; j++)
            {
                if (items[j].ID == id)
                {
                    ItemData data = slots[j].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount--;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    if (data.amount == 0)
                    {
                        Destroy(slots[j].transform.GetChild(0).gameObject);
                        items[j] = new Item();
                        break;
                    }
                    if (data.amount == 1)
                    {
                        slots[j].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                        break;
                    }
                    break;
                }
            }
        }
        else
            for (int i = 0; i < items.Count; i++)
                if (items[i].ID != -1 && items[i].ID == id)
                {
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                    items[i] = new Item();
                    break;
                }
    }
}
