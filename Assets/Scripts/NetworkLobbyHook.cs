using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook_PlayerSetup : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponentInChildren<LobbyPlayer>();
        NetworkPlayerSetup localPlayer = gamePlayer.GetComponent<NetworkPlayerSetup>();

        localPlayer.pName = lobby.playerName;
        localPlayer.playerColor = lobby.playerColor;
    }
}
