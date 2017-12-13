using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class RadioPuzzle : MonoBehaviour
{
    public Camera puzzleCamera; // Camera for the puzzle
    public GameObject[] switches = new GameObject[2]; // Radio power buttons
    public GameObject scrollWheel; // Radio wheel button
    public GameObject pointer; // GameObject that indicates the current frequency
    public AudioClip[] audios = new AudioClip[5];
    public AudioSource audioSource;
    public AudioSource interferenceSource;
    public GameObject antenna;
    public GameObject microphone;
    public Transform bestAntennaPosition; // Interference is smaller when antenna is close to this position
    public bool antennaInPlace = false; // If the player holding the antenna is in the right place

    private GameObject activePlayer = null; // The player that is currently infront of the radio
    private int playingAudio = 5; // Playing station code
    private bool puzzleActive = false; // Is player standing in puzzle area
    private bool radioOn = false; // Are both switches on
    private int switchOnCount = 0; // Amount of switches that are on
    private bool changeFrequency = false; // If wheel button is pressed it gives permission to use changefrequency
    private float lastMousePosition; // Used for changefrequency to track mousemovement
    private float volume; // audio volume
    private bool puzzleDone = false; // When puzzle is done
    private bool _guiEnable = false;
    public string _text;

    private void Update()
    {
        if(puzzleActive && activePlayer != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                CastRay();
            }
            else if(Input.GetMouseButton(0) && changeFrequency)
            {
                ChangeFrequancy();
            }
            else if(Input.GetMouseButtonUp(0) && changeFrequency)
            {
                changeFrequency = !changeFrequency;
            }
        }
        if (radioOn)
        {
            SetAudioVolumes();
            PlaySound();
        }
    }

    private void CastRay() //Radio button interaction
    {
        RaycastHit hit;
        if (Physics.Raycast(puzzleCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            int index = 0;
            foreach (GameObject o in switches)
            {
                if (hit.transform.gameObject == o)
                {
                    activePlayer.GetComponent<NetworkRadio>().UseSwitch(GetComponent<NetworkIdentity>().netId, index);
                }
                ++index;
            }
            if (hit.transform.gameObject == scrollWheel)
            {
                changeFrequency = true;
                lastMousePosition = Input.mousePosition.x;
            }
            if(hit.transform.gameObject == microphone)
            {
                if (puzzleDone)
                {
                    _text = "The second rescue team has reached its first destination, over.";
                    StartCoroutine(ShowText());
                }
                else if (!puzzleDone && playingAudio == 3 && volume != 1)
                {
                    _text = "Signal is not strong enough.";
                    StartCoroutine(ShowText());
                }
                else if (!puzzleDone && playingAudio == 3 && volume == 1)
                {
                    _text = "Antenna is getting too much interference.";
                    StartCoroutine(ShowText());
                }
                else
                {
                    _text = "This is not the right station.";
                    StartCoroutine(ShowText());
                
                }
            }
        }
    }


    IEnumerator ShowText()
    {
        _guiEnable = true;
        yield return new WaitForSeconds(3f);
        _guiEnable = false;
    }


    void OnGUI()
    {
        if (_guiEnable != false)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), _text);
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 50, 50), " ");
        }
    }

    private void ChangeFrequancy() // Moves the pointer and the scrollWheel
    {
        int direction;
        float dragDistance = Mathf.Abs(lastMousePosition - Input.mousePosition.x);
        if (lastMousePosition < Input.mousePosition.x) direction = -1;
        else direction = 1;

        Vector3 t = pointer.transform.localPosition;
        float amount = Mathf.Clamp(t.x + direction * dragDistance / 100 * Time.deltaTime, -0.29f, 0.055f);
        float rotateAmount = -direction * dragDistance / 5;

        pointer.transform.localPosition = new Vector3(amount, t.y, t.z);

        if (amount > -0.29f && amount < 0.055f)
        {
            scrollWheel.transform.Rotate(Vector3.forward, rotateAmount);
        }

        ChangeAudio(amount);


        lastMousePosition = Input.mousePosition.x;
    }

    private void ChangeAudio(float amount) // Selects right audio and volume
    {
        float a = (amount - (-0.29f)) * (100 / 0.345f); //pointerPos normalized between 0-100
        float mid = 0;
        if (a > 0 && a < 5)
        {
            playingAudio = 0;
            mid = 2.5f;
        }
        else if (a > 15 && a < 20)
        {
            playingAudio = 1;
            mid = 17.5f;
        }
        else if (a > 35 && a < 40)
        {
            playingAudio = 2;
            mid = 37.5f;
        }
        else if (a > 65 && a < 70)
        {
            playingAudio = 3;
            mid = 67.5f;
        }
        else if (a > 90 && a < 95)
        {
            playingAudio = 4;
            mid = 92.5f;
        }
        else
        {
            playingAudio = 5;
        }

        float vol = (2.5f - Mathf.Abs(mid - a)) / 2.5f;
        if (vol > 0.90f) vol = 1;
        if (vol < 0) vol = 0;
        volume = vol;
        activePlayer.GetComponent<NetworkRadio>().SetAudioInfo(GetComponent<NetworkIdentity>().netId, playingAudio, volume);
    }

    private void SetAudioVolumes() // Sets volumes to audios and counts Antenna interference, sets puzzleDone
    {
        float addInterference = 0.8f;
        if(antennaInPlace && antenna != null)
        {
            float dist = Vector3.Distance(antenna.transform.position, bestAntennaPosition.position);
            if(dist < 10)
            {
                addInterference = dist / 12;
                if (addInterference < 0.2f) addInterference = 0;
            }
        }
        audioSource.volume = Mathf.Clamp(volume - addInterference, 0, 1);
        interferenceSource.volume = Mathf.Clamp((1-volume) + addInterference, 0, 1);

        if (volume - addInterference >= 1 && playingAudio == 3)
        {
            puzzleDone = true;
            Debug.Log("Puzzle Done");
        }
        else puzzleDone = false;
    }

    private void PlaySound() // Makes sure that the right audio is playing
    {
        if (playingAudio < audios.Length)
        {
            if (audioSource.clip != audios[playingAudio])
            {
                audioSource.clip = audios[playingAudio];
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.PlayDelayed(0.4f);
            }
        }
        else audioSource.clip = null;
    }

    private void OnTriggerEnter (Collider other) // Changes camera to puzzleCamera
    {
        if(puzzleActive == false && other.CompareTag("Player"))
        {
            Transform hands = other.transform.Find("FirstPersonCharacter").Find("HANDS");
            Debug.Log(hands);
            activePlayer = other.gameObject;
            hands.gameObject.SetActive(false);
            puzzleCamera.enabled = true;
            other.GetComponentInChildren<Camera>().enabled = true;
            other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(false);
            activePlayer.GetComponent<NetworkRadio>().ActivatePuzzle(GetComponent<NetworkIdentity>().netId, true);
        }
    }

    private void OnTriggerExit (Collider other) // Changes back to playersCamera
    {
        if(activePlayer != null && other.CompareTag("Player"))
        {
            Transform hands = other.transform.Find("FirstPersonCharacter").Find("HANDS");
            hands.gameObject.SetActive(true);
            activePlayer.GetComponent<NetworkRadio>().TurnWheel(GetComponent<NetworkIdentity>().netId, pointer.transform.localPosition, scrollWheel.transform.localEulerAngles);
            activePlayer = null;
            other.GetComponentInChildren<Camera>().enabled = true;
            puzzleCamera.enabled = false;
            other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(true);
            other.GetComponent<NetworkRadio>().ActivatePuzzle(GetComponent<NetworkIdentity>().netId, false);
        }
    }

    public void ActivatePuzzle(bool state) // All clients activate the puzzle
    {
        puzzleActive = state;
        if (true) interferenceSource.Play();
    }

    public void UseSwitch(int index)
    {
        Vector3 t = switches[index].transform.localRotation.eulerAngles;
        switches[index].transform.localRotation = Quaternion.Euler(-t.x, t.y, t.z);
        if (t.x < 10) ++switchOnCount;
        else --switchOnCount;
        if (switchOnCount == 2)
        {
            radioOn = true;
            audioSource.mute = false;
            interferenceSource.mute = false;
        }
        else
        {
            radioOn = false;
            audioSource.mute = true;
            interferenceSource.mute = true;
        }
    } // All clients turn switches and set on the radio

    public void TurnWheel(Vector3 p, Vector3 w) // All clients move the pointer and turn the wheelbutton
    {
        pointer.transform.localPosition = p;
        scrollWheel.transform.localEulerAngles = w;
    }

    public void SetAudioInfo(int audio, float vol) // All clients change their playing audio and volume
    {
        playingAudio = audio;
        volume = vol;
    }

    public void AntennaInPlace(bool state, GameObject obj) // All clients set antennainplace and antenna
    {
        antennaInPlace = state;
        antenna = obj;
    }
}
