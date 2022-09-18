using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopGun : MonoBehaviour
{
    [Header("Armory")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform fireStartPoint;
    private Character character;

    [Header("Stats")]
    [SerializeField]
    private float bulletSpeed;

    private void Start()
    {
        character = this.GetComponent<Character>();
    }

    // Fires the projectile
    public void FireProjectile()
    {
        Vector2 direction = (fireStartPoint.transform.position * Vector2.right);
        Projectile tempProj = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder).GetComponent<Projectile>();
        if (tempProj != null)
            tempProj.InitProjectile(bulletSpeed * direction, character, false);
    }
}
