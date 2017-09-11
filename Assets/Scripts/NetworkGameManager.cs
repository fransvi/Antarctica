using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkGameManager : NetworkBehaviour
{
    static public List<NetworkPlayerSetup> players = new List<NetworkPlayerSetup>();
    static public NetworkGameManager sInstance = null;

    [Header("Gameplay")]
    public GameObject[] spawnableObjects;

    protected bool _spawningGameobject = true;
    protected bool _running = true;
    private bool playerDead;


    void Awake()
    {
        sInstance = this;
    }

    void Start()
    {
        if (isServer)
        {
            StartCoroutine(GameTimer());
        }

        for(int i = 0; i < players.Count; ++i)
        {
            //TODO: Initialize player data if needed for each player.
            //players[i].Init();
        }
    }

    [ServerCallback]
    void Update()
    {
        if (!_running || players.Count == 0)
        {
            return;
        }
        for (int i = 0; i < players.Count; ++i)
        {
            if(players[i].playerHealth <= 0)
            {
                playerDead = true;
            }
        }
        if(playerDead)
        {
            StartCoroutine(ReturnToLobby());
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        //TODO: Possible registered prefabs in game
        foreach (GameObject obj in spawnableObjects)
        {
            ClientScene.RegisterPrefab(obj);
        }
    }

    IEnumerator ReturnToLobby()
    {
        _running = false;
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ActivateCursor();
        //TODO: Make Gameover screen to replace following to throw players out when one is dead
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }

    IEnumerator GameTimer()
    {
        //TODO: runtime logic/timed events
        yield return new WaitForSeconds(5);
    }


    public IEnumerator WaitForRespawn(NetworkPlayer player)
    {
        yield return new WaitForSeconds(4.0f);

        //TODO: possible player respawns
        //player.respawn()
    }
}
