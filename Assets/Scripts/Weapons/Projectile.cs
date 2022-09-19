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
    private bool damagesEnemies = false;
    [SerializeField]
    private Character attacker;
    private Rigidbody2D rb;

    // Initializes the projectile
    public void InitProjectile(Vector2 velocity, Character _attacker, bool _damagesEnemies, int _damage = 1, bool _isEnemy = true)
    {
        rb = this.GetComponent<Rigidbody2D>();
        attacker = _attacker;
        damage = _damage;
        isEnemy = _isEnemy;
        damagesEnemies = _damagesEnemies;
        rb.velocity = velocity;
    }

    // Handles collision between the projectile and it's target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.collider.CompareTag("Player") && isEnemy) || (collision.collider.CompareTag("Enemy") && (!isEnemy || damagesEnemies)))
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(new DamageInfo(damage, DamageType.HAZARD, attacker));
                Destroy(this.gameObject);
            }
        }

        // Destroy projectile on collision with friend cube
        if (collision.collider.CompareTag("Friend") || collision.collider.CompareTag("Ground"))
            Destroy(this.gameObject);
    }
}
