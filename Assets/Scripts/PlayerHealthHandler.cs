using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerHealthHandler : MonoBehaviour
{

    [Header("Player Properties")]
    public SpriteRenderer _playerSpriteRenderer;
    public int _playerHealth;
    [Header("Object Assignments")]
    public TextMeshPro _healthText;

    public void SetPlayerHealth(int hp) => _playerHealth = hp;
    public void ChangeHealth(int hp) => _playerHealth = Mathf.Max(_playerHealth + hp, 0);
    public bool IsAlive() => _playerHealth > 0;

    private PhotonView _photonView;
    private const float _damageInvulnerabilityTime = 0.05f;
    private float _timeSinceLastDamage = 0;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        SetPlayerHealth(10);
        UpdateHealthText(_playerHealth);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // We only want this for the player, so we know if they took damage.
        if (!_photonView.IsMine) { return; }
        if (Time.time - _timeSinceLastDamage < _damageInvulnerabilityTime) { return; }
        // If touching a bullet, take damage!
        if (col.GetComponent<BulletHandler>()?.OwnerViewID != _photonView.ViewID)
        {
            TakeDamage(1);
            _timeSinceLastDamage = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
        _photonView.RPC("UpdateHealthText", RpcTarget.AllBuffered, _playerHealth);
    }

    [PunRPC]
    public void UpdateHealthText(int hp)
    {
        _playerHealth = hp;
        _healthText.text = hp + " HP";
        // If the player is dead, make them turn a weird color.
        if (!IsAlive())
        {
            _playerSpriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         // We own this player: send the others our data
    //         stream.SendNext(this._playerHealth);
    //     }
    //     else
    //     {
    //         // Network player, receive data
    //         this._playerHealth = (int)stream.ReceiveNext();
    //     }
    // }

}
