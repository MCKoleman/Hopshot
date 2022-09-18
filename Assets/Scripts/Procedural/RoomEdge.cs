using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEdge : MonoBehaviour
{
    public delegate void RoomComplete();
    public static event RoomComplete OnRoomComplete;

    private Room parentRoom;
    private bool hasBeenActivated = false;

    private void Start()
    {
        parentRoom = this.GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only count player collisions
        if (!collision.CompareTag("Player"))
            return;

        hasBeenActivated = true;
        parentRoom.CompleteRoom();
        OnRoomComplete?.Invoke();
        this.gameObject.SetActive(false);
    }
}
