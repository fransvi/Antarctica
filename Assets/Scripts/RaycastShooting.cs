using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

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
    public Image _textImage;

    public bool hasKey = false;
    public bool hasKeyCode = false;
    private bool _guiEnable = false;
    public string _showText;
    public float waitTime = 3.0f;

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
                    door.OpenDoor(hasKey, hasKeyCode);
                }
 
                break;
            case 3:
                NetworkServer.Destroy(obj); //Itemi lootattu tuhotaan scenestä
                break;
            case 4:
                if (obj.GetComponent<PlayerHealth>().isKnockedDown)
                {
                    obj.GetComponent<PlayerHealth>().Revive();
                }
                else
                {
                }
                break;

            case 5:
                hasKey = true;
                NetworkServer.Destroy(obj);
                break;
            case 6:
                hasKeyCode = true;
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
                    _textImage.gameObject.SetActive(true);
                    GetComponent<PlayerHealth>().Note2.text = "Found a message from the previous team in the book.";
                    _textImage.gameObject.GetComponentInChildren<Text>().text = h.transform.gameObject.GetComponent<InteractableObject>()._itemText;
                    viewingText = true;
                }
                else
                {
                    _textImage.gameObject.GetComponentInChildren<Text>().text = "";
                    GetComponent<FirstPersonController>().MouseLockFPSC = false;
                    //h.transform.gameObject.GetComponent<InteractableObject>().ShowText(false, _textImage);
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
                    h.transform.GetComponent<DoorController>().OpenDoor(hasKey, hasKeyCode);
                    _showText = h.transform.GetComponent<DoorController>()._doorInfo;
                    StartCoroutine(ShowText());
                }
                
            }
            if (h.transform.CompareTag("Item"))
            {
                int id = h.transform.GetComponent<ItemPick>().id;
                StopAllCoroutines();
                CmdHitObject(3, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                inv.AddItem(id);
                switch (id)
                {
                    case 0:
                        _showText = "Compass picked up.";
                        StartCoroutine(ShowText());
                        break;
                    case 1:
                        _showText = "Lantern picked up.";
                        StartCoroutine(ShowText());
                        break;
                    case 2:
                        _showText = "Antenna picked up.";
                        StartCoroutine(ShowText());
                        break;
                    case 3:
                        _showText = "Choco picked up.";
                        StartCoroutine(ShowText());
                        break;
                    case 4:
                        _showText = "Termos picked up.";
                        StartCoroutine(ShowText());
                        break;
                    default:
                        break;
                }

                

            }
            if (h.transform.CompareTag("Player"))
            {
                CmdHitObject(4, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
            }
            if (h.transform.CompareTag("Key"))
            {
                _showText = "Key picked up.";
                StartCoroutine(ShowText());
                GetComponent<PlayerHealth>().Note1.text = "Found a key from the housing unit.";
                CmdHitObject(5, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
            }
            if (h.transform.CompareTag("KeyCode"))
            {
                _showText = "Keycode picked up.";
                StartCoroutine(ShowText());
                GetComponent<PlayerHealth>().Note3.text = "Found a numpad code on the wall: 1853.";
                CmdHitObject(6, h.transform.gameObject.GetComponent<NetworkIdentity>().netId);
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

    public IEnumerator ShowText()
    {
        //StopAllCoroutines();
        _guiEnable = true;
        itemtext();
        yield return new WaitForSeconds(waitTime);
        _guiEnable = false;
        itemtext();
    }

     public void itemtext()
    {
        if((_guiEnable != false) && (_showText != ""))
        {
            _textImage.gameObject.SetActive(true);

            _textImage.gameObject.GetComponentInChildren<Text>().text = _showText;
            _textImage.gameObject.GetComponentInChildren<Text>().resizeTextForBestFit = true;

            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 640, 480), _showText);
        }
        else
        {
           // _textImage.gameObject.SetActive(false);
            _textImage.gameObject.GetComponentInChildren<Text>().text = " ";
            _textImage.gameObject.GetComponentInChildren<Text>().resizeTextForBestFit = false;
            //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 640, 480), " ");
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                ShootRayCast();
            }
        }

    }


    private IEnumerator ShotEffect()
    {

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}