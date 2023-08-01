using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public enum WeaponType
{
    PISTOL = 0, SHOTGUN = 1
}

public class PlayerWeaponHandler : MonoBehaviour
{

    [Header("Prefab Assignments")]
    public GameObject bulletPrefab;

    private WeaponType _weaponType = WeaponType.PISTOL;
    private Vector2 _initialScale;
    private PhotonView _photonView;
    private float _timeToWaitBeforeNextShot = 0;

    // Swap to the next available weapon.
    // Does this by simply adding one to the enum and wrapping around to zero.
    public void SwapWeapon()
    {
        _weaponType = Enum.IsDefined(typeof(WeaponType), _weaponType + 1) ? _weaponType + 1 : 0;
        ChooseWeapon(_weaponType);
    }

    // Make this player equip a specific weapon.
    public void ChooseWeapon(WeaponType weaponType)
    {
        // Switch to the weapon.
        _weaponType = weaponType;
        // Update the user interface.
        UIManager.Instance.UpdateWeaponText(Enum.GetName(typeof(WeaponType), _weaponType));
    }

    private void Awake()
    {
        _initialScale = transform.localScale;
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        // All players start with a pistol.
        ChooseWeapon(WeaponType.PISTOL);
    }

    private void Update()
    {
        // If this isn't the local player, ignore this.
        if (!_photonView.IsMine) { return; }
        // If the player is dead, ignore this.
        if (!PlayerManager.Instance.IsAlive()) { return; }
        // Let the player only shoot when the cooldown has been released from previous shots.
        _timeToWaitBeforeNextShot = Mathf.Max(0, _timeToWaitBeforeNextShot - Time.deltaTime);
        if (Input.GetMouseButtonDown(0) && _timeToWaitBeforeNextShot <= 0)
        {
            Vector3 mousePos = GetWorldPositionOnPlane(Input.mousePosition, 0) - transform.position;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            _photonView.RPC("ShootBullet", RpcTarget.All, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)), _weaponType);
        }
        // If space key is pressed, let player swap their weapon.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapWeapon();
        }
    }

    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    [PunRPC]
    private void ShootBullet(Vector3 position, Quaternion rotation, WeaponType weaponType)
    {
        GameObject bulletObj = null;
        switch (weaponType)
        {
            case WeaponType.PISTOL:
                bulletObj = Instantiate(bulletPrefab);
                bulletObj.GetComponent<BulletHandler>().Initialize(position, rotation, _photonView.ViewID);
                _timeToWaitBeforeNextShot = 0.2f;
                break;
            case WeaponType.SHOTGUN:
                for (int i = -2; i <= 2; i++)
                {
                    Quaternion adjustedRotation = rotation * Quaternion.Euler(0, 0, i * 16);
                    bulletObj = Instantiate(bulletPrefab);
                    bulletObj.GetComponent<BulletHandler>().Initialize(position, adjustedRotation, _photonView.ViewID);
                    _timeToWaitBeforeNextShot = 0.8f;
                }
                break;
        }
    }

}
