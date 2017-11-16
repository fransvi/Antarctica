using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {


    public ParticleSystem _snowParticles;
    public ParticleSystem _fogParticles;
    public ParticleSystem _stars;
    public float _windForce;
	// Use this for initialization
	void Start () {

        _snowParticles = transform.Find("SnowParticleSystem").gameObject.GetComponent<ParticleSystem>();
        _fogParticles = transform.Find("FogParticleSystem").gameObject.GetComponent<ParticleSystem>();
        _stars = transform.Find("Stars").gameObject.GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {

            var rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            transform.rotation = rotation;

    }
}
