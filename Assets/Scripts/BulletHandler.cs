using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{

    public int OwnerViewID;

    public void Initialize(Vector3 position, Quaternion rotation, int viewID)
    {
        OwnerViewID = viewID;
        transform.position = position;
        transform.rotation = rotation;
        StartCoroutine(FireBulletCoroutine());
    }

    private IEnumerator FireBulletCoroutine(int ticksRendered = 0)
    {
        transform.Translate(new Vector3(0.1f, 0, 0), Space.Self);
        yield return new WaitForSeconds(0.02f);
        if (ticksRendered < 100)
        {
            StartCoroutine(FireBulletCoroutine(++ticksRendered));
        }
    }

    // When this bullet collides, destroy it.
    public void OnTriggerEnter2D(Collider2D col)
    {
        // If it's not a player object, ignore it.
        if (!col.TryGetComponent<PlayerManager>(out PlayerManager playerManager)) { return; }
        // If the player is dead, ignore it.
        if (!playerManager.IsAlive()) { return; }
        // If the source of this bullet is what the bullet is colliding with, ignore.
        if (OwnerViewID == playerManager.photonView.ViewID) { return; }
        Destroy(gameObject, 0.01f);
    }

}
