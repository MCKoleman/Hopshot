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
    private float latestX = float.MinValue;
    private PlayerController player;
    private float initialY;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        initialY = this.transform.position.y;
    }

    private void Update()
    {
        if (!shouldFollow)
            return;

        this.transform.position = Vector3.Lerp(this.transform.position, GetTargetPos(), followSpeed * Time.deltaTime);
        latestX = this.transform.position.x;
    }

    // Returns a valid target position to move to
    private Vector3 GetTargetPos()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        return new Vector3(Mathf.Max(player.GetPos().x, latestX), initialY, this.transform.position.z);
    }
}
