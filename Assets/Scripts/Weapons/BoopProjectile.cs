using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopProjectile : MonoBehaviour
{
    [SerializeField]
    private float boopForce = 1.0f;
    [SerializeField]
    private float playerBoopMod = 0.5f;
    [SerializeField]
    private float playerAirBoopMod = 0.3f;
    [SerializeField]
    private float friendBoopMod = 10.0f;
    [SerializeField]
    private float enemyBoopMod = 5.0f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only collide with Players, Enemies, and Friends
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy") && !collision.CompareTag("Friend"))
            return;

        // Apply boop modifiers depending on the object hit by the boop
        float totalBoopForce = boopForce;
        if (collision.CompareTag("Player"))
        {
            totalBoopForce *= playerBoopMod;

            // Reduce boop force when the player is in the air
            PlayerController tempPlayer = collision.GetComponent<PlayerController>();
            if (tempPlayer != null && !tempPlayer.IsTouchingGround)
                totalBoopForce *= playerAirBoopMod;
        }
        else if (collision.CompareTag("Enemy"))
            totalBoopForce *= enemyBoopMod;
        else if (collision.CompareTag("Friend"))
            totalBoopForce *= friendBoopMod;

        // Only affect components with rigidbodies
        Rigidbody2D tempRB = collision.GetComponent<Rigidbody2D>();
        if (tempRB == null)
            return;

        // Boops the rigidbody away from the projectile with inverse square force
        Vector2 distance = tempRB.position - rb.position;
        tempRB.velocity += totalBoopForce / Mathf.Pow(distance.magnitude, 2.0f) * distance.normalized;
    }
}
