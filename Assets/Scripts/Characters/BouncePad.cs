using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField]
    private float bounceForce;
    [SerializeField]
    private float maxBounceForce = 20.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Player") && !collision.CompareTag("Friend"))
            return;

        // Find Rigidbody
        Rigidbody2D tempRB = collision.GetComponent<Rigidbody2D>();
        if (tempRB == null)
            return;

        Debug.Log($"Bouncing [{collision.name}] with force [{Mathf.Abs(bounceForce * tempRB.velocity.y)}]");
        tempRB.velocity = new Vector2(tempRB.velocity.x, Mathf.Min(Mathf.Abs(bounceForce * tempRB.velocity.y), maxBounceForce));
    }
}