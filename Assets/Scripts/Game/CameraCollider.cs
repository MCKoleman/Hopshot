using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public delegate void CameraCollide();
    public delegate void CameraUncollide();
    public static event CameraCollide OnCameraCollide;
    public static event CameraUncollide OnCameraUncollide;

    private bool isPlayerColliding = false;
    private Transform player;
    private const float DEATH_THRESHOLD = 0.02f;

    private void Start()
    {
        this.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, this.transform.position.y, this.transform.position.z);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        // Only check for player death if they are still colliding
        if (!isPlayerColliding || !GameManager.Instance.IsGameActive || player == null)
            return;

        if (Camera.main.WorldToViewportPoint(player.position).x <= DEATH_THRESHOLD)
            GameManager.Instance.HandlePlayerDeath();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy enemies that collide
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            return;
        }

        if (!collision.collider.CompareTag("Player"))
            return;

        isPlayerColliding = true;
        OnCameraCollide?.Invoke();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        isPlayerColliding = false;
        OnCameraUncollide?.Invoke();
    }
}
