using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType { DEFAULT = 0, EASY = 1, HARD = 3 }

    [SerializeField]
    private GameObject ground;
    [SerializeField]
    private GameObject nextRoomNode;
    [SerializeField]
    private Transform spawnNodeHolder;
    [SerializeField]
    private List<ContentNode> spawnNodes = new List<ContentNode>();
    [SerializeField]
    private int roomNum = -1;
    [SerializeField]
    private RoomType roomType;
    [SerializeField]
    private bool isComplete = false;

    private void Start()
    {
        if (spawnNodeHolder == null)
            return;

        // Add all nodes from the spawn node holder to the content list
        foreach(Transform temp in spawnNodeHolder)
            spawnNodes.Add(temp.GetComponent<ContentNode>());
    }

    // Initializes all the rooms components
    public void InitRoom(RoomType _type = RoomType.DEFAULT, int _roomNum = -1)
    {
        // Set room info
        roomNum = _roomNum;
        roomType = _type;

        // Only spawn content if there is any
        if (spawnNodes == null || spawnNodes.Count == 0)
            return;

        // Spawn all content
        foreach(ContentNode node in spawnNodes)
            node.InitContent();
    }

    // Completes the room
    public void CompleteRoom()
    {
        isComplete = true;
    }

    // Returns the next rooms position
    public Vector3 GetNextRoomPos() { return nextRoomNode.transform.position; }
    public RoomType GetRoomType() { return roomType; }
}
