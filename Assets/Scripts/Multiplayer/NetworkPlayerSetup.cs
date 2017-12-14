using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkPlayerSetup : NetworkBehaviour
{

    public Behaviour[] componentsToEnable;
    public Camera playerCamera;
    public GameObject playerUi;
    public int playerHealth;
    public bool inVehicle;
    public GameObject worldModel;
    public GameObject viewModel;
    public GameObject weatherSystem;
    public GameObject introCanvas;
    public GameObject endCanvas;
    public GameObject radio;
    public bool isClients;
    private bool endDisplayed = false;
    public bool isServers;
    [SyncVar]
    public string pName = "player";

    [SyncVar]
    public Color playerColor = Color.white;

    void Update()
    {
        isClients = isClient;
        isServers = isServer;
        //Might cause lag TODO: better implementation
        if (inVehicle)
        {
            
        }
        if (radio.GetComponent<RadioPuzzle>().puzzleDone && !endDisplayed)
        {
            endDisplayed = true;
            StartCoroutine(DisplayEndCanvas());

        }

        
    }


    void Start()
    {

        //Placeholder :: Set colour for player 
        weatherSystem = GameObject.Find("WeatherSystem");
        Renderer[] rend = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend)
        {
            //r.material.color = playerColor;
        }
        if (!isLocalPlayer)
        {
            weatherSystem.SetActive(false);
            this.GetComponent<CharacterController>().enabled = false;
            viewModel.SetActive(false);

        }
        radio = GameObject.Find("Radio");
    }

    public void ReloadWorldModels()
    {
        if (isLocalPlayer)
        {
            worldModel.SetActive(false);
        }
        else
        {
            worldModel.SetActive(true);
        }
    }

    public void DisableWorldModel()
    {
        worldModel.SetActive(false);
    }


    IEnumerator DisplayIntroCanvas()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        Image image = introCanvas.transform.Find("Image").GetComponent<Image>();
        Text text = introCanvas.transform.Find("Text").GetComponent<Text>();
        Text text2 = introCanvas.transform.Find("Text2").GetComponent<Text>();
        
        introCanvas.SetActive(true);
        text2.CrossFadeAlpha(0f, 0f, false);
        text.CrossFadeAlpha(0f, 0f, false);
        text.CrossFadeAlpha(1f, 2f, false);
        yield return new WaitForSeconds(3f);
        text.CrossFadeAlpha(0f, 1f, false);
        yield return new WaitForSeconds(1f);
        text2.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(5f);
        text2.CrossFadeAlpha(0f, 2f, false);
        image.CrossFadeAlpha(0f, 2f, false);
        yield return new WaitForSeconds(3f);
        introCanvas.SetActive(false);

    }

    IEnumerator DisplayEndCanvas()
    {
        endCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        endCanvas.SetActive(false);

    }
    public override void OnStartLocalPlayer()
    {

        gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
        playerCamera = GetComponentInChildren<Camera>();
        //Enable Components
        if (GameObject.Find("GameEndCanvas"))
        {
            endCanvas = GameObject.Find("GameEndCanvas");
            endCanvas.SetActive(false);
        }

        if (GameObject.Find("IntroCanvas"))
        {
            StartCoroutine(DisplayIntroCanvas());
        }

        foreach (Behaviour component in componentsToEnable)
        {
            component.enabled = true;
        }
        worldModel.SetActive(false);
        //Enable Camera
        playerCamera.enabled = true;
        playerCamera.GetComponent<AudioListener>().enabled = true;
        //Enable CharacterController
        this.GetComponent<CharacterController>().enabled = true;

        //Renderer[] rens = GetComponentsInChildren<Renderer>();
        //foreach(Renderer ren in rens)
        //{
        //    ren.enabled = false;
        //}

        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);

 
    }

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

    public void Init()
    {
        //Player init through Gamemanager
    }

    public void EnableLobbyCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    void Awake()
    {
        NetworkGameManager.players.Add(this);
    }

    void OnDestroy()
    {
        NetworkGameManager.players.Remove(this);
    }



}
