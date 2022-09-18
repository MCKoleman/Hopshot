using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private bool isEnemy = true;
    [SerializeField]
    private Character attacker;
    private Rigidbody2D rb;

    // Initializes the projectile
    public void InitProjectile(Vector2 velocity, Character _attacker, int _damage = 1, bool _isEnemy = true)
    {
        rb = this.GetComponent<Rigidbody2D>();
        attacker = _attacker;
        damage = _damage;
        isEnemy = _isEnemy;
        rb.velocity = velocity;
    }

    // Handles collision between the projectile and it's target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.collider.CompareTag("Player") && isEnemy) || (collision.collider.CompareTag("Enemy") && !isEnemy))
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(new DamageInfo(damage, DamageType.HAZARD, attacker));
                Destroy(this.gameObject);
            }
        }
    }
}
