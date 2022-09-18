using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOP_gun : MonoBehaviour
{
    [Header("Armory")]
    [SerializeField]
    private Transform spawn;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float chargeTime;

    private float chargeUp;
    private bool canShoot = true;

    private void Update()
    {
        Reload();
        if (canShoot)
            Shoot();
    }

    private void Shoot()
    {
        Vector2 direction = (spawn.transform.position * Vector2.right) * -1.0f;
        GameObject loadShot = Instantiate(bullet, spawn.position, Quaternion.identity);
        loadShot.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
        chargeUp = Time.time + chargeTime;
    }

    private void Reload()
    {
        if(Time.time < chargeUp)
            canShoot = false;
        else
            canShoot = true;
    }
}
