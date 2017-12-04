using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAnimationManager : MonoBehaviour {

    private GameObject kompassi;
    private GameObject lyhty;
    private GameObject suklaa;
    private GameObject termos;
    private Animator fpsAnimator;
    private PlayerMovementScript pms;

    string itemTaken;
    bool syncing;
    int frameCount;

	// Use this for initialization
	void Start () {

        fpsAnimator = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        pms = GetComponent<PlayerMovementScript>();
        kompassi = transform.GetChild(0).GetChild(2).GetChild(12).gameObject;
        lyhty = transform.GetChild(0).GetChild(2).GetChild(8).gameObject;
        suklaa = transform.GetChild(0).GetChild(2).GetChild(9).gameObject;
        termos = transform.GetChild(0).GetChild(2).GetChild(11).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
        if (syncing)
        {
            frameCount++;

            if (frameCount % 10 == 0)
            {
                syncing = false;
                frameCount = 0;
                SyncItemHoldingAnimations(itemTaken);
            }
        }
	}

    public void EmptyHands()
    {
        itemTaken = "";
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);
    }

    public void ResetIdleAnimInstantly()
    {
        fpsAnimator.Play("RightHandEmptyIdle", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
        fpsAnimator.Play("HandLeft|IdleEmptyRight", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
    }

    public void SyncItemHoldingAnimations(string item)
    {
        if (item == "Lantern")
        {
            fpsAnimator.Play("HandRight|LyhtyIdle(Hand)", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
            fpsAnimator.Play("HandLeft|IdleEmptyRight", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
            fpsAnimator.Play("Lyhty|LyhtyIdle(lyhty)", fpsAnimator.GetLayerIndex("LyhtyLayer"), 0f);
        }
        else if (item == "Compass")
        {
            fpsAnimator.Play("HandRight|KompassiIdle(Hand)", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
            fpsAnimator.Play("HandLeft|IdleEmptyRight", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
            fpsAnimator.Play("Kompassi|KompassiIdle(kompassi)", fpsAnimator.GetLayerIndex("KompassiLayer"), 0f);
        }
        else if (item == "Chocolate")
        {
            fpsAnimator.Play("HandRight|KompassiIdle(Hand)", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
            fpsAnimator.Play("HandLeft|IdleEmptyRight", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
            fpsAnimator.Play("SuklaaPatukka|SuklaaIdle", fpsAnimator.GetLayerIndex("SuklaaLayer"), 0f);
        }
        else if (item == "Thermos")
        {
            fpsAnimator.Play("HandRight|KompassiIdle(Hand)", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
            fpsAnimator.Play("HandLeft|IdleEmptyRight", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
            fpsAnimator.Play("Armature|TermosIdle(Termos)", fpsAnimator.GetLayerIndex("TermosLayer"), 0f);
        }
    }

    public void IdleHands()
    {
        fpsAnimator.SetBool("Moving", false);
    }

    public void ResetItemHold()
    {
        fpsAnimator.SetBool("LyhtyActive", false);
        fpsAnimator.SetBool("KompassiActive", false);
    }

    public void HandsMove()
    {
        fpsAnimator.SetBool("Moving", true);
    }

    public void TakeLantern()
    {
        itemTaken = "Lantern";
        kompassi.SetActive(false);
        lyhty.SetActive(true);
        suklaa.SetActive(false);
        termos.SetActive(false);

        fpsAnimator.Play("otaLyhty", fpsAnimator.GetLayerIndex("LyhtyLayer"), 0f);
        fpsAnimator.SetBool("LyhtyActive", true);
        syncing = true;
    }

    public void TakeCompass()
    {
        Invoke("ActivateTheCompass", 0.25f);
        itemTaken = "Compass";
        //kompassi.SetActive(true);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);

        fpsAnimator.Play("otaKompassi", fpsAnimator.GetLayerIndex("KompassiLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
        syncing = true;
    }

    public void ActivateTheCompass()
    {
        kompassi.SetActive(true);
    }

    public void TakeChocolate()
    {
        itemTaken = "Chocolate";
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(true);
        termos.SetActive(false);

        fpsAnimator.Play("otaSuklaa", fpsAnimator.GetLayerIndex("SuklaaLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
        syncing = true;
    }

    public void TakeThermos()
    {
        itemTaken = "Thermos";
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(true);

        fpsAnimator.Play("otaTermos", fpsAnimator.GetLayerIndex("TermosLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
        syncing = true;
    }

    public void SetLantern()
    {
        fpsAnimator.SetTrigger("SaadaValoa");
    }

    public void Eat()
    {
        pms.allowItemChange = false;
        fpsAnimator.Play("SuklaaPatukka|Syönti(SUklaa)", fpsAnimator.GetLayerIndex("SuklaaLayer"), 0f);
        Invoke("ResetItemHold", 0.5f);
        Invoke("EmptyHands", 0.5f);
        Invoke("ResetIdleAnimInstantly", 0.5f);
        Invoke("AllowItemChangeAgain", 0.5f);
    }

    public void Drink()
    {
        pms.allowItemChange = false;
        fpsAnimator.Play("Armature|Juonti(Termos)", fpsAnimator.GetLayerIndex("TermosLayer"), 0f);
        Invoke("ResetItemHold", 0.5f);
        Invoke("EmptyHands", 0.5f);
        Invoke("ResetIdleAnimInstantly", 0.5f);
        Invoke("AllowItemChangeAgain", 0.5f);
    }

    void AllowItemChangeAgain()
    {
        pms.allowItemChange = true;
    }

    void AllowClickAndTwistAgain()
    {
        pms.allowClick = true;
        pms.allowTwist = true;
    }

    public void Click()
    {
        pms.allowClick = false;
        pms.allowTwist = false;
        Debug.Log("click activated");
        pms.allowItemChange = false;
        fpsAnimator.Play("napinPaino", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
        Invoke("ResetIdleAnimInstantly", 0.8f);
        Invoke("AllowItemChangeAgain", 0.8f);
        Invoke("AllowClickAndTwistAgain", 0.8f);
    }

    public void Twist()
    {
        pms.allowClick = false;
        pms.allowTwist = false;
        Debug.Log("twist activated");
        pms.allowItemChange = false;
        fpsAnimator.Play("napinVääntö", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
        Invoke("ResetIdleAnimInstantly", 1.2f);
        Invoke("AllowItemChangeAgain", 1.2f);
        Invoke("AllowClickAndTwistAgain", 1.2f);
    }
}
