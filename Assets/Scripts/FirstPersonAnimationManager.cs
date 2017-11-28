using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAnimationManager : MonoBehaviour {

    private GameObject kompassi;
    private GameObject lyhty;
    private GameObject suklaa;
    private GameObject termos;
    private Animator fpsAnimator;

    string itemTaken;
    bool syncing;
    int frameCount;

	// Use this for initialization
	void Start () {

        fpsAnimator = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        kompassi = transform.GetChild(0).GetChild(2).GetChild(6).gameObject;
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
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);
    }

    public void ResetIdleAnimInstantly()
    {
        fpsAnimator.Play("RightHandEmptyIdle", fpsAnimator.GetLayerIndex("RightHandLayer"), 0f);
        fpsAnimator.Play("LeftHandEmptyIdle", fpsAnimator.GetLayerIndex("LeftHandLayer"), 0f);
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
        itemTaken = "Compass";
        kompassi.SetActive(true);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);

        fpsAnimator.Play("otaKompassi", fpsAnimator.GetLayerIndex("KompassiLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
        syncing = true;
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
        fpsAnimator.SetTrigger("Syo");
        // delay, sitten resetItemHold() ja EmptyHands()
    }

    public void Click()
    {
        fpsAnimator.SetTrigger("Klikkaus");
    }

    public void Twist()
    {
        fpsAnimator.SetTrigger("Vaanto");
    }

}
