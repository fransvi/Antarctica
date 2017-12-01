using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkRadio : NetworkBehaviour
{
    [Command]
    void CmdActivatePuzzle (NetworkInstanceId id, bool state)
    {
        RpcActivatePuzzle(id, state);
    }

    [ClientRpc]
    void RpcActivatePuzzle (NetworkInstanceId id, bool state)
    {
        ClientScene.FindLocalObject(id).GetComponent<RadioPuzzle>().ActivatePuzzle(state);
    }

    public void ActivatePuzzle (NetworkInstanceId id, bool state)
    {
        CmdActivatePuzzle(id, state);
    }

    [Command]
    void CmdUseSwitch(NetworkInstanceId id, int index)
    {
        RpcUseSwitch(id, index);
    }

    [ClientRpc]
    void RpcUseSwitch(NetworkInstanceId id, int index)
    {
        ClientScene.FindLocalObject(id).GetComponent<RadioPuzzle>().UseSwitch(index);
    }

    public void UseSwitch(NetworkInstanceId id, int index)
    {
        CmdUseSwitch(id, index);
    }

    [Command]
    void CmdTurnWheel(NetworkInstanceId id, Vector3 p, Vector3 w)
    {
        RpcTurnWheel(id, p, w);
    }

    [ClientRpc]
    void RpcTurnWheel(NetworkInstanceId id, Vector3 p, Vector3 w)
    {
        ClientScene.FindLocalObject(id).GetComponent<RadioPuzzle>().TurnWheel(p, w);
    }

    public void TurnWheel(NetworkInstanceId id, Vector3 p, Vector3 w)
    {
        CmdTurnWheel(id, p, w);
    }

    [Command]
    void CmdSetAudioInfo(NetworkInstanceId id, int audio, float vol)
    {
        RpcSetAudioInfo(id, audio, vol);
    }

    [ClientRpc]
    void RpcSetAudioInfo(NetworkInstanceId id, int audio, float vol)
    {
        ClientScene.FindLocalObject(id).GetComponent<RadioPuzzle>().SetAudioInfo(audio, vol);
    }

    public void SetAudioInfo(NetworkInstanceId id, int audio, float vol)
    {
        CmdSetAudioInfo(id, audio, vol);
    }

    [Command]
    void CmdAntennaInPlace(NetworkInstanceId id, bool state, NetworkInstanceId antennaId)
    {
        RpcAntennaInPlace(id, state, antennaId);
    }

    [ClientRpc]
    void RpcAntennaInPlace(NetworkInstanceId id, bool state, NetworkInstanceId antennaId)
    {
        GameObject player = ClientScene.FindLocalObject(antennaId);
        GameObject obj = player.transform.Find("Equipment").Find("Antenna(Clone)").gameObject;
        ClientScene.FindLocalObject(id).GetComponent<RadioPuzzle>().AntennaInPlace(state, obj);
    }

    public void AntennaInPlace(NetworkInstanceId id, bool state, NetworkInstanceId antennaId)
    {
        CmdAntennaInPlace(id, state, antennaId);
    }

}
