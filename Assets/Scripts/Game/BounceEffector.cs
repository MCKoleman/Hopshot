using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffector : MonoBehaviour
{
    private const float MIN_ANGLE = 45.0f;
    private Rigidbody2D rb;
    private List<GameObject> collidingObjs = new List<GameObject>();
    public bool IsAgainstWall { get { return collidingObjs.Count != 0; } }
    public float GetWallDir { get { return (collidingObjs.Count == 0) ? 1.0f : ((collidingObjs[0].transform.position.x - this.transform.position.x >= 0.0f) ? 1.0f : -1.0f); } }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only affect collisions with ground
        if (!collision.collider.CompareTag("Ground") && collision.collider.GetInstanceID() != this.gameObject.GetInstanceID())
            return;

        if (Vector2.Angle(Vector2.up, collision.GetContact(0).normal) < MIN_ANGLE)
            return;

        if (!collidingObjs.Contains(collision.gameObject))
            collidingObjs.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Only affect collisions with ground
        if (!collision.collider.CompareTag("Ground"))
            return;

        if(collidingObjs.Contains(collision.gameObject))
            collidingObjs.Remove(collision.gameObject);
    }
}
