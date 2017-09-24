using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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



    void Start()
    {
        laserLine = GetComponent<LineRenderer>();

        fpsCam = GetComponentInParent<Camera>();

        isGrabbed = false;
    }

    public void CmdShootRayCast()
    {


        nextFire = Time.time + fireRate;

        StartCoroutine(ShotEffect());

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        //RaycastHit hit;

        laserLine.SetPosition(0, gunEnd.position);

        RaycastHit[] hits;

        hits = Physics.RaycastAll(rayOrigin, fpsCam.transform.forward, weaponRange);

        foreach (RaycastHit h in hits)
        {
            if (h.transform.CompareTag("Interactable"))
            {

                h.rigidbody.AddForce(-h.normal * hitForce);
                laserLine.SetPosition(1, h.point);
            }
            if (h.transform.CompareTag("Door"))
            {
                Debug.Log("Door hit");
                
                if (h.transform.GetComponent<Animator>().GetBool("isOpen"))
                {
                    h.transform.GetComponent<DoorController>().CloseDoor();
                }
                else
                {
                    h.transform.GetComponent<DoorController>().OpenDoor();
                }
                


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
                CmdShootRayCast();
            }

    }


    private IEnumerator ShotEffect()
    {

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}