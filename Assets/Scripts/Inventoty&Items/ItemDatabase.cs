using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    private List<Item> database = new List<Item>(); //Lista esineistä tallennetaan tähän
    private JsonData itemData; //Esineen data Json muodossa

	void Start () {

        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json")); // Muutetaan esineiden Jsondata string muotoon Items.json tiedostosta
        ConstructItemDatabase();//Kutsutaan rakennusfunktiota
        Debug.Log(FetchItemById(0).Description);
	}

    //Etsii db:stä tietyn esineen käyttämällä id:tä
    public Item FetchItemById(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
                return database[i];
        }

        return null;
    }

    void ConstructItemDatabase()
    {
        //Käy läpi kaikki Item.json tiedostossa olevat esineet
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["title"].ToString(), float.Parse(itemData[i]["weight"].ToString()),
                itemData[i]["description"].ToString(),(bool)itemData[i]["stackable"],itemData[i]["slug"].ToString()));
        }


    }
}

public class Item
{
    //Alla esineen ominaisuudet ja konstruktori ja tyhjä konstruktori, jonka ID on -1
    public int ID { get; set; }
    public string Title { get; set; }
    public float Weight { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set;}
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string title , float weight,string description,bool stackable, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Weight = weight;
        this.Description = description;
        this.Stackable = stackable;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("ItemSprites/Items/"+ slug);
    }
    public Item()
    {
        this.ID = -1;
    }
}
