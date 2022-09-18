using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)]
    private float followSpeed;
    [SerializeField]
    private bool shouldFollow = false;
    [SerializeField]
    private float moveSpeed = 0.1f;
    [SerializeField]
    private float minScreenPos = 0.70f;
    [SerializeField]
    private float latestX = float.MinValue;

    private PlayerController player;
    private float initialY;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        initialY = this.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!shouldFollow)
            return;

        latestX = Mathf.Lerp(this.transform.position.x, Mathf.Max(GetTargetX(), latestX + GetMoveDisplacement()), followSpeed * Time.fixedDeltaTime);
        this.transform.position = new Vector3(latestX, initialY, this.transform.position.z);
    }

    // Returns a valid target position to move to
    private float GetTargetX()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        // If the target hasn't reached the move threshold, keep the camera still
        if (GetTargetPosPercent() <= minScreenPos)
            return this.transform.position.x;

        // If the target has reached the move threshold, use the player's position
        return player.GetPos().x;
    }

    // Returns the targets position as a percentage of its position on screen
    private float GetTargetPosPercent()
    {
        return Camera.main.WorldToViewportPoint(player.GetPos()).x;
    }
    
    // Moves the camera slowly to the right
    private float GetMoveDisplacement()
    {
        return moveSpeed * Time.fixedDeltaTime * SpawnManager.Instance.GetCameraMoveMod();
    }
}
