using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomList", menuName = "ScriptableObjects/RoomList", order = 1)]
[System.Serializable]
public class RoomList : ScriptableObject
{
    [SerializeField]
    private GameObject startRoom;
    [SerializeField]
    private WeightedGameObjectList easyRooms;
    [SerializeField]
    private WeightedGameObjectList hardRooms;

    public GameObject GetStartRoom()
    {
        return startRoom;
    }

    public GameObject GetRandomEasyRoom()
    {
        return easyRooms.GetRandomObject();
    }

    public GameObject GetRandomHardRoom()
    {
        return hardRooms.GetRandomObject();
    }
}
