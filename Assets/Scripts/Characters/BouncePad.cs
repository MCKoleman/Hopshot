using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField]
    private float bounce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Friend"))
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if (tempChar != null)
                {
                Rigidbody2D tempRB = tempChar.GetComponent<Rigidbody2D>();
                if (tempRB != null)
                {
                    Vector2 tempVector = tempRB.velocity;
                    float flipY = bounce * tempVector.y;
                    if (flipY < 0)
                        flipY *= -1;

                    tempVector.y = flipY;
                    tempRB.velocity = tempVector; 

                    Debug.Log("Bounce");
                }
            }
        }
    }
}