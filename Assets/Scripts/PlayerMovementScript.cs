using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerMovementScript : NetworkBehaviour {

    public Animator playerAnimator;


	// Use this for initialization
	void Start () {


    }

    //Timer for jump animation
    IEnumerator JumpAnimation()
    {
        playerAnimator.SetBool("isJumping", true);
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("isJumping", false);
    }
	
	// Update is called once per frame
	void Update () {

        if (gameObject.layer == LayerMask.NameToLayer("LocalPlayer")) {


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
            }
            // if any movement button is pressed without left shift, our character walks
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                playerAnimator.SetBool("isWalking", true);
            }
            // sidestep animations added
            else if(Input.GetKey(KeyCode.A))
            {
                playerAnimator.SetBool("isSidesteppingL", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                playerAnimator.SetBool("isSidesteppingR", true);
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
                }

            }
            // when left-shift is lifted, no more running
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                playerAnimator.SetBool("isRunning", false);
            }

            

        }

	}
}
