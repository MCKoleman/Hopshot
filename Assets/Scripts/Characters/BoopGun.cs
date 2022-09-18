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
    [SerializeField]
    private Transform fireTowards;

    private Character character;

    [Header("Stats")]
    [SerializeField]
    private float bulletSpeed;

    private void Start()
    {
        character = this.GetComponent<Character>();
    }

    // Fires the projectile
    public Vector2 FireProjectile()
    {
        Vector2 direction = (fireTowards.transform.position - fireStartPoint.transform.position) * -1.0f;
        Projectile tempProj = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder).GetComponent<Projectile>();
        if (tempProj != null)
            tempProj.InitProjectile(bulletSpeed * direction, character, true);
        else
            return Vector2.zero;
        return direction;
    }
}
