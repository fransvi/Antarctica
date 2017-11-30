using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsScript : MonoBehaviour {

    public Transform sun;
    public GameObject fpscon;
    Vector3 tvec;
    public Transform worldProbe;

    // Use this for initialization
    void Start()
    {
        Transform[] transforms = transform.parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == "FirstPersonCharacter")
            {
                fpscon = t.gameObject;
            }
        }
        tvec = fpscon.transform.position;
        tvec.y = -200f;
    }

    bool lighton = false;

    // Update is called once per frame
    void Update()
    {
        sun = GameObject.Find("Sun").transform;
        transform.rotation = sun.transform.rotation;
        worldProbe.transform.position = tvec;


    }
}
