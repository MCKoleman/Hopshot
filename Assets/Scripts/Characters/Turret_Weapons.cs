using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Weapons : MonoBehaviour
{
    public delegate void Gun();
    public event Gun StartFire;
    public event Gun StopFire;

    private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        StartFire?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        StopFire?.Invoke();
    }
}

