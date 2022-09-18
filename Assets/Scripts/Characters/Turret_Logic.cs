using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Logic : MonoBehaviour
{
    [SerializeField]
    private Turret_Weapons gun;

    [SerializeField]
    private Target_Light led;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform spawn;

    [SerializeField]
    private float bulletSpeed;
    private float correctPath = 1.0f;
    float nextShot = 0.0f;

    [SerializeField]
    private bool faceLeft;
    private bool fire;

    private void Update()
    {
        if(fire && Time.time > nextShot)
        {
            Shoot();
            nextShot = Time.time + .5f;
        }
    }

    private void OnEnable()
    {
        gun.StartFire += Wake;
        gun.StopFire += Sleep;

        if(faceLeft)
            correctPath = -1.0f;
    }

    private void OnDisable()
    {
        gun.StartFire -= Wake;
        gun.StopFire -= Sleep;
    }

    private void Wake()
    {
        led.SetColor(Color.red);
        fire = true;
    }

    private void Sleep()
    {
        led.SetColor(Color.green);
        fire = false;
    }

    void Shoot()
    {
        Vector2 direction = (gun.transform.position - spawn.transform.position) * correctPath;
        GameObject loadShot = Instantiate(bullet, spawn.position, Quaternion.identity);
        loadShot.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
    }
}
