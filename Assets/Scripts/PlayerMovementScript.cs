using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerMovementScript : NetworkBehaviour {

    public Animator playerAnimator;
    private Animator fpsAnimator;
    private FirstPersonAnimationManager fpsAnimManager;
    public bool allowItemChange;
    public bool allowItemoff;
    public bool allowClick;
    public bool allowTwist;

    public bool allowMovement;

    [SerializeField]
    private string itemHeld;

	// Use this for initialization
	void Start () {

        fpsAnimator = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        fpsAnimManager = GetComponent<FirstPersonAnimationManager>();
        allowItemChange = true;
        allowItemoff = false;
        allowClick = true;
        allowTwist = true;
        allowMovement = true;
    }

    //Timer for jump animation
    IEnumerator JumpAnimation()
    {
        playerAnimator.SetBool("isJumping", true);
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("isJumping", false);
    }
	

    [Command]
    void CmdEquipLantern(NetworkInstanceId netId, bool b)
    {
        RpcEquipLantern(netId, b);
    }
    [ClientRpc]
    void RpcEquipLantern(NetworkInstanceId netId, bool b)
    {
        if (b)
        {
            GameObject localPlayer = ClientScene.FindLocalObject(netId);
            localPlayer.GetComponent<Animator>().SetBool("lanternActive", true);
            localPlayer.GetComponent<FirstPersonAnimationManager>().ResetItemHold();
            localPlayer.GetComponent<FirstPersonAnimationManager>().ResetIdleAnimInstantly();
            itemHeld = "Lantern";
            localPlayer.GetComponent<FirstPersonAnimationManager>().TakeLantern();
        }
        else
        {
            GameObject localPlayer = ClientScene.FindLocalObject(netId);
            localPlayer.GetComponent<Animator>().SetBool("lanternActive", true);
            localPlayer.GetComponent<FirstPersonAnimationManager>().ResetItemHold();
            localPlayer.GetComponent<FirstPersonAnimationManager>().ResetIdleAnimInstantly();
            localPlayer.GetComponent<FirstPersonAnimationManager>().EmptyHands();
            itemHeld = "";
  
        }

       
    }
	// Update is called once per frame
	void Update () {

        if (isLocalPlayer) {

            if (allowMovement)
            {

                if (Input.GetKeyDown(KeyCode.Alpha1) && itemHeld != "Lantern" && allowItemChange)
                {
                    CmdEquipLantern(GetComponent<NetworkIdentity>().netId, true);
                    /*
            playerAnimator.SetBool(lanternActive, true);
           fpsAnimManager.ResetItemHold();
           fpsAnimManager.ResetIdleAnimInstantly();
           itemHeld = "Lantern";
           fpsAnimManager.TakeLantern();
           */

                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) && itemHeld == "Lantern")
                {
                    CmdEquipLantern(GetComponent<NetworkIdentity>().netId, false);
                    /*
                    playerAnimator.SetBool("lanternActive", false);
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    fpsAnimManager.EmptyHands();
                    itemHeld = "";
                    */
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && itemHeld != "Compass" && allowItemChange)
                {
                    playerAnimator.SetBool("lanternActive", false);
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    itemHeld = "Compass";
                    fpsAnimManager.TakeCompass();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && itemHeld == "Compass")
                {
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    fpsAnimManager.EmptyHands();
                    itemHeld = "";
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && itemHeld != "Chocolate" && allowItemChange)
                {
                    playerAnimator.SetBool("lanternActive", false);
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    itemHeld = "Chocolate";
                    fpsAnimManager.TakeChocolate();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && itemHeld == "Chocolate")
                {
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    fpsAnimManager.EmptyHands();
                    itemHeld = "";
                }
                if (Input.GetKeyDown(KeyCode.Alpha4) && itemHeld != "Thermos" && allowItemChange)
                {
                    playerAnimator.SetBool("lanternActive", false);
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    itemHeld = "Thermos";
                    fpsAnimManager.TakeThermos();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && itemHeld == "Thermos")
                {
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    fpsAnimManager.EmptyHands();
                    itemHeld = "";
                }
                if (Input.GetKeyDown(KeyCode.Alpha5) && itemHeld != "Antenna" && allowItemChange)
                {
                    playerAnimator.SetBool("lanternActive", false);
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    itemHeld = "Antenna";
                    //fpsAnimManager.TakeThermos();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && itemHeld == "Antenna")
                {
                    fpsAnimManager.ResetItemHold();
                    fpsAnimManager.ResetIdleAnimInstantly();
                    fpsAnimManager.EmptyHands();
                    itemHeld = "";
                }
                //Check jump before anything
                if (Input.GetKey(KeyCode.Space))
                {
                    StartCoroutine(JumpAnimation());
                }
                //if any movement button is pressed with left shift, our character runs
                else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) ||
                    Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
                {
                    //playerAnimator.SetBool("isWalking", false);
                    playerAnimator.SetBool("isRunning", true);
                    fpsAnimator.SetBool("Moving", true);
                }
                // if any movement button is pressed without left shift, our character walks
                else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    playerAnimator.SetBool("isWalking", true);
                    fpsAnimator.SetBool("Moving", true);
                }
                // sidestep animations added
                else if (Input.GetKey(KeyCode.A))
                {
                    playerAnimator.SetBool("isSidesteppingL", true);
                    fpsAnimator.SetBool("Moving", true);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    playerAnimator.SetBool("isSidesteppingR", true);
                    fpsAnimator.SetBool("Moving", true);
                }
                // when any movement button is lifted, its checked wether any movement button is still held down or not. If not, controller return to idle.
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    {
                        playerAnimator.SetBool("isWalking", false);
                        playerAnimator.SetBool("isRunning", false);
                        playerAnimator.SetBool("isSidesteppingR", false);
                        playerAnimator.SetBool("isSidesteppingL", false);
                        fpsAnimator.SetBool("Moving", false);
                    }
                }
                // when left-shift is lifted, no more running
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    playerAnimator.SetBool("isRunning", false);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!fpsAnimator.GetCurrentAnimatorStateInfo(4).IsName("otaSuklaa") && !fpsAnimator.GetCurrentAnimatorStateInfo(4).IsName("SuklaaPatukka|Syönti(SUklaa)") && itemHeld == "Chocolate")
                    {
                        playerAnimator.SetTrigger("eat");
                        fpsAnimManager.Eat();
                    }
                    else if (!fpsAnimator.GetCurrentAnimatorStateInfo(5).IsName("otaTermos") && !fpsAnimator.GetCurrentAnimatorStateInfo(5).IsName("Armature|Juonti(Termos)") && itemHeld == "Thermos")
                    {
                        playerAnimator.SetTrigger("eat");
                        fpsAnimManager.Drink();
                    }
                    else if (!fpsAnimator.GetCurrentAnimatorStateInfo(0).IsName("napinPaino") && itemHeld == "" && allowClick)
                    {
                        fpsAnimManager.Click();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    if (!fpsAnimator.GetCurrentAnimatorStateInfo(0).IsName("napinVääntö") && itemHeld == "" && allowTwist)
                    {
                        fpsAnimManager.Twist();
                    }
                }
            }
        }
	}

    public void ResetItems()
    {
        playerAnimator.SetBool("lanternActive", false);
        fpsAnimManager.ResetItemHold();
        fpsAnimManager.ResetIdleAnimInstantly();
        fpsAnimManager.EmptyHands();
        itemHeld = "";
    }

    public void ActivateOrDisable()
    {
        if (itemHeld != "Lantern" && allowItemChange)
        {
            playerAnimator.SetBool("lanternActive", true);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            itemHeld = "Lantern";
            fpsAnimManager.TakeLantern();
        }
        else if (itemHeld == "Lantern")
        {
            playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            itemHeld = "";
        }
    }

    public void ActivateOrDisableCompass()
    {
        if (itemHeld != "Compass" && allowItemChange)
        {
            playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            itemHeld = "Compass";
            fpsAnimManager.TakeCompass();
        }
        else if (itemHeld == "Compass")
        {
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            itemHeld = "";
        }
    }

    public void ActivateOrDisableAntenna()
    {
        if (itemHeld != "Antenna" && allowItemChange)
        {
            playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            itemHeld = "Antenna";
            //fpsAnimManager.TakeCompass();
        }
        else if (itemHeld == "Antenna")
        {
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            itemHeld = "";
        }
    }

    public void ActivateOrDisableChocolate()
    {
        if (itemHeld != "Chocolate" && allowItemChange)
        {
            playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            itemHeld = "Chocolate";
            fpsAnimManager.TakeChocolate();
        }
        else if (itemHeld == "Chocolate")
        {
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            itemHeld = "";
        }
    }

    public void ActivateOrDisableTermos()
    {
        if (itemHeld != "Thermos" && allowItemChange)
        {
            playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            itemHeld = "Thermos";
            fpsAnimManager.TakeThermos();
        }
        else if (itemHeld == "Thermos")
        {
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            itemHeld = "";
        }
    }

    public void CheckItemAnimation(int id, bool Itemonoff)
    {

        playerAnimator.SetBool("lanternActive", false);
        fpsAnimManager.ResetItemHold();
        fpsAnimManager.ResetIdleAnimInstantly();
        fpsAnimManager.EmptyHands();


        if ((allowItemChange == false) && (Itemonoff == true))
        {
            //playerAnimator.SetBool("lanternActive", false);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.EmptyHands();
            allowItemChange = true;
            itemHeld = "";
            return;
        }
        if ((id == 0))
        {
            fpsAnimManager.TakeCompass();
            allowItemChange = false;
            return;
        }
        if ((id == 1))
        {
            playerAnimator.SetBool("lanternActive", true);
            fpsAnimManager.ResetItemHold();
            fpsAnimManager.ResetIdleAnimInstantly();
            fpsAnimManager.TakeLantern();
            itemHeld = "Lantern";
            allowItemChange = false;
        }

        if ((id == 3))
        {
            fpsAnimManager.TakeChocolate();
            allowItemChange = false;
        }

        if ((id == 4))
        {
            fpsAnimManager.TakeThermos();
            allowItemChange = false;
        }

    }
}
