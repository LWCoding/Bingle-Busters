using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(PlayerHealthHandler))]
[RequireComponent(typeof(PlayerMovementHandler))]
[RequireComponent(typeof(PlayerWeaponHandler))]
public class PlayerManager : MonoBehaviourPunCallbacks
{

    public static PlayerManager Instance;
    [Header("Object Assignments")]
    public TextMeshPro usernameText;

    private PhotonView _photonView;
    private PlayerHealthHandler _playerHealthHandler;

    public bool IsAlive() => _playerHealthHandler.IsAlive();

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _playerHealthHandler = GetComponent<PlayerHealthHandler>();
        gameObject.tag = (_photonView.IsMine) ? "Player" : "Enemy";
        // If this is the local player, set this to be the static instance.
        if (_photonView.IsMine)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetUsername();
    }

    public void SetUsername()
    {
        usernameText.text = _photonView.Owner.NickName;
    }

}
