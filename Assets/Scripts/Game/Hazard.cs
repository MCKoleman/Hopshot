using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if(tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}
