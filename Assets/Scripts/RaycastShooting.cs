using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class RaycastShooting : NetworkBehaviour
{

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private LineRenderer laserLine;
    private float nextFire;

    private RaycastHit vision;
    public float rayLength;
    private bool isGrabbed;
    private Rigidbody grabbedObject;

    private Inventory inv;

    private bool viewingText;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();

        fpsCam = GetComponentInChildren<Camera>();

        isGrabbed = false;

        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == "Inventory")
            {
                inv = t.gameObject.GetComponent<Inventory>();
            }
        }

    }

    [Command]
    void CmdHitObject(int id, NetworkInstanceId netId)
    {
        RpcHitObject(id, netId);
    }
    [ClientRpc]
    void RpcHitObject(int id, NetworkInstanceId netId)
    {
        GameObject obj = ClientScene.FindLocalObject(netId);
        switch (id)
        {
            case 1:
                if (obj.GetComponent<LightSwitch>())
                {
                    LightSwitch light = obj.GetComponent<LightSwitch>();
                    if (light._isActive)
                    {
                        light.Activate(false);
                    }
                    else
                    {
                        light.Activate(true);
                    }
                }
                else if (obj.GetComponent<PowerGeneratorScript>())
                {
                    PowerGeneratorScript pg = obj.GetComponent<PowerGeneratorScript>();
                    if (pg._powerOn)
                    {
                        pg.DisablePower();
                    }
                    else
                    {
                        pg.EnablePower();
                    }
                }

                break;
            case 2:
                DoorController door = obj.GetComponent<DoorController>();
                if (door._isOpen)
                {
                    door.CloseDoor();
                }
                else
                {
                    door.OpenDoor();
                }
 
                break;
            case 3:
                NetworkServer.Destroy(obj); //Itemi lootattu tuhotaan scenestä
                break;
            default:
                
                break;
        }
    }

    public void ShootRayCast()
    {


        nextFire = Time.time + fireRate;

        StartCoroutine(ShotEffect());

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        //RaycastHit hit;

        laserLine.SetPosition(0, gunEnd.position);

        RaycastHit[] hits;

        hits = Physics.RaycastAll(rayOrigin, fpsCam.transform.forward, weaponRange);

        //RaycastHittien toiminnat idllä:
        //0 = Yleinen
        //1 = Valo
        //2 = Ovi
        //3 = Item
        foreach (RaycastHit h in hits)
        {
            if (h.transform.CompareTag("Interactable"))
            {
                if (!viewingText)
                {
                    GetComponent<FirstPersonController>().MouseLockFPSC = true;
                    h.transform.gameObject.GetComponent<InteractableObject>().ShowText(true);
                    viewingText = true;
                }
                else
                {
                    GetComponent<FirstPersonController>().MouseLockFPSC = false;
                    h.transform.gameObject.GetComponent<InteractableObject>().ShowText(false);
                    viewingText = false;
                }
                //h.transform.gameObject.GetComponent<InteractableObject>().ShowText(true);
                
            }
            if (h.transform.CompareTag("Switch"))
            {
                CmdHitObject(1, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                if (h.transform.gameObject.GetComponent<PowerGeneratorScript>())
                {
                    PowerGeneratorScript pg = h.transform.gameObject.GetComponent<PowerGeneratorScript>();
                    if (pg._powerOn)
                    {
                        pg.DisablePower();
                    }
                    else
                    {
                        pg.EnablePower();
                    }
                }
                laserLine.SetPosition(1, h.point);
            }
            if (h.transform.CompareTag("Door"))
            {
                CmdHitObject(2, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                laserLine.SetPosition(1, h.point);
                
                if (h.transform.GetComponent<Animator>().GetBool("isOpen"))
                {
                    h.transform.GetComponent<DoorController>().CloseDoor();
                }
                else
                {
                    h.transform.GetComponent<DoorController>().OpenDoor();
                }
                
            }
            if (h.transform.CompareTag("Item"))
            {
                CmdHitObject(3, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                inv.AddItem(h.transform.GetComponent<ItemPick>().id);

            }

            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            
        }


        /*
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);
            EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.Damage(gunDamage);
            }

            
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
        }
        */
    }


    void Update()
    {
 
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                ShootRayCast();
            }

    }


    private IEnumerator ShotEffect()
    {

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}