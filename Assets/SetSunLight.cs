using UnityEngine;
using System.Collections;

public class SetSunLight : MonoBehaviour
{

    Material sky;

    public Renderer water;

    public Transform stars;
    public Transform worldProbe;

    // Use this for initialization
    void Start()
    {

        sky = RenderSettings.skybox;

    }

    bool lighton = false;

    // Update is called once per frame
    void Update()
    {

        stars.transform.rotation = transform.rotation;

        Vector3 tvec = Camera.main.transform.position;
        worldProbe.transform.position = tvec;

        water.GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset = new Vector2(Time.time / 100, 0);

        //water depth map--
        //water.GetComponent<MeshRenderer>().GetComponent<Material>().SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 80));

    }
}