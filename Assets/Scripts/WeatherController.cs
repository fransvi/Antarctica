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

    }
	
	// Update is called once per frame
	void Update () {

            var rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            transform.rotation = rotation;

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "IgnoreSnow")
        {
            _snowParticles.Stop();
            _snowParticles.Clear();

            _fogParticles.Stop();
            _fogParticles.Clear();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "IgnoreSnow")
        {
            _snowParticles.Play();
            _fogParticles.Play();
        }
    }
}
