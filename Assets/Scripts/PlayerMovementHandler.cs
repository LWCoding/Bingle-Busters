using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementHandler : MonoBehaviour
{

    [Header("Player Properties")]
    public float moveSpeed;

    private PhotonView _photonView;
    private Rigidbody2D _rb2D;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (_photonView.IsMine)
        {
            _rb2D.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed,
                                        Input.GetAxis("Vertical") * moveSpeed), ForceMode2D.Impulse);
        }
    }

}
