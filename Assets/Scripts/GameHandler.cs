using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameHandler : MonoBehaviourPunCallbacks
{

    [Header("Prefab Assignments")]
    public GameObject playerPrefab;

    #region UNITY

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }

    #endregion

}
