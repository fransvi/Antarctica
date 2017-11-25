using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsScript : MonoBehaviour {

    public Transform sun;

    // Use this for initialization
    void Start()
    {
    }

    bool lighton = false;

    // Update is called once per frame
    void Update()
    {
        sun = GameObject.Find("Sun").transform;
        transform.rotation = sun.transform.rotation;


    }
}
