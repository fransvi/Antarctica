using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

    private GameObject actionbar;
    private Item _item;
    public bool equiped;
    private ItemData data;
    private Inventory inv;
    private PlayerHealth health;

    public Button euipButton;
    public Button dropButton;
    public Button consumeButton;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Halutaaan pääsy inventory objektiin
        actionbar = GameObject.Find("ActionBar");
        actionbar.SetActive(false);
        equiped = false;

        Button btn = euipButton.GetComponent<Button>();
        Button btn2 = dropButton.GetComponent<Button>();
        Button btn3 = consumeButton.GetComponent<Button>();
        btn.onClick.AddListener(() => { EquipItem(_item);});
        btn2.onClick.AddListener(() => { DropItem(_item);});
        btn3.onClick.AddListener(() => { ConsumeItem(_item); });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){

            this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyReduceHealth(20);
            
            //health.InstantlyReduceHealth(1);
        }
    }

    public void Activate (Item item)
    {

        _item = item;
        actionbar.SetActive(true);
        actionbar.transform.position = Input.mousePosition;

    }

	public void Deactivate ()
    {

        actionbar.SetActive(false);

    }
    void EquipItem(Item item)
    {
        _item = item;
        Transform apu = GameObject.Find("Equipment").transform;
        if (equiped == false && item.ID != apu.GetChild(0).GetComponent<ItemPick>().id)
        {
            GameObject itemtoequip = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu);
            Debug.Log(_item.Title);
            apu.parent.GetComponent<PlayerEquip>()._lantern = itemtoequip;
            equiped = true;
        }
        else if (equiped == true)
        {
            Destroy(apu.GetChild(0).gameObject);
            equiped = false;
        }
    }

    void ConsumeItem(Item item)
    {
        Debug.Log("Consumed");
        //_item = item;
        //Transform apu = GameObject.Find("Equipment").transform;

        this.gameObject.GetComponentInParent<PlayerHealth>().InstantlyIncreaseHealth(item.Healthamount);
        Debug.Log("Heatlh incfeased" + item.Healthamount);


        inv.RemoveItem(item.ID);
        actionbar.SetActive(false);

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

        GameObject itemtodrop = Instantiate((GameObject)Resources.Load("Prefabs/" + _item.Title), apu.transform.position + Vector3.forward, Quaternion.identity);
    }
}
