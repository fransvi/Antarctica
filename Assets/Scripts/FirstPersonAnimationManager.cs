using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAnimationManager : MonoBehaviour {

    private GameObject kompassi;
    private GameObject lyhty;
    private GameObject suklaa;
    private GameObject termos;
    private Animator fpsAnimator;

	// Use this for initialization
	void Start () {

        fpsAnimator = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        kompassi = transform.GetChild(0).GetChild(2).GetChild(6).gameObject;
        lyhty = transform.GetChild(0).GetChild(2).GetChild(8).gameObject;
        suklaa = transform.GetChild(0).GetChild(2).GetChild(9).gameObject;
        termos = transform.GetChild(0).GetChild(2).GetChild(11).gameObject;

        TakeChocolate();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EmptyHands()
    {
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);
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
        kompassi.SetActive(false);
        lyhty.SetActive(true);
        suklaa.SetActive(false);
        termos.SetActive(false);

        fpsAnimator.Play("otaLyhty", fpsAnimator.GetLayerIndex("LyhtyLayer"), 0f);
        fpsAnimator.SetBool("LyhtyActive", true);
    }

    public void TakeCompass()
    {
        kompassi.SetActive(true);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(false);

        fpsAnimator.Play("otaKompassi", fpsAnimator.GetLayerIndex("KompassiLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
    }

    public void TakeChocolate()
    {
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(true);
        termos.SetActive(false);

        fpsAnimator.Play("otaSuklaa", fpsAnimator.GetLayerIndex("SuklaaLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
    }

    public void TakeThermos()
    {
        kompassi.SetActive(false);
        lyhty.SetActive(false);
        suklaa.SetActive(false);
        termos.SetActive(true);

        fpsAnimator.Play("otaTermos", fpsAnimator.GetLayerIndex("TermosLayer"), 0f);
        fpsAnimator.SetBool("KompassiActive", true);
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
