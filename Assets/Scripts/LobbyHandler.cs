using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class LobbyHandler : MonoBehaviourPunCallbacks
{

    [Header("UI Assignments")]
    public TMP_InputField roomCodeInput;

    public void JoinRoom()
    {
        PhotonNetwork.NickName = roomCodeInput.text;
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinOrCreateRoom("main", null, null);
        }
        else
        {
            Debug.Log("Already in room!");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room! " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Game");
    }

}
