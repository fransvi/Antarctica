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
            slots[i].transform.SetParent(slotPanel.transform);//asetetaan uudet slotit olemaan slotPanelin lapsia
        }
        AddItem(0);
        AddItem(1);
    }

    //Lisää esineen jolle annetaan esineen id
    public void AddItem(int id)
    {
       
        Item itemToAdd = database.FetchItemById(id);// täyttää kyseisen esineobjektin sillä kyseisellä id:llä olevan esineen databasesta
        Debug.Log(itemToAdd.Slug);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)//tsekkaa onko esine olemsassa
            {
                items[i] = itemToAdd;//vie kyseisen inventoryn paikkaan haetun esineen
                GameObject itemObj = Instantiate(inventoryItem); //luo fyysisen kopion esineprefabista
                itemObj.transform.SetParent(slots[i].transform); //Asettaa slotin olemaan esineen lapsi
                itemObj.transform.position = Vector2.zero;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;// Asettaa prefabin source imagen olemaan esineen nimen mukainen Sprite
                itemObj.name = itemToAdd.Title;// Asettaa slotissa olevien esineiden nimeksi databasessa olevan nimen. Tämä näkyy inspectorissa

                break;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
