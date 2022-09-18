using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Logic : MonoBehaviour
{
    [SerializeField]
    private Turret_Weapons gun;

    [SerializeField]
    private Target_Light led;

    private void OnEnable()
    {
        gun.StartFire += Wake;
        gun.StopFire += Sleep;
    }

    private void OnDisable()
    {
        gun.StartFire -= Wake;
        gun.StopFire -= Sleep;
    }

    private void Wake()
    {
        led.SetColor(Color.red);
    }

    private void Sleep()
    {
        led.SetColor(Color.green);
    }

}
