using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)]
    private float followSpeed;
    [SerializeField]
    private bool shouldFollow = false;
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!shouldFollow)
            return;

        this.transform.position = Vector3.Lerp(this.transform.position, GetPlayerPos(), followSpeed * Time.deltaTime);
    }

    private Vector3 GetPlayerPos()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        return new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
    }
}
