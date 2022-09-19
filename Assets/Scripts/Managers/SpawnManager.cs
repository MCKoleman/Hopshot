using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Spawn Info")]
    [SerializeField]
    private RoomList roomList;
    [SerializeField]
    private ContentList contentList;
    [SerializeField]
    private DifficultyMods diffMods;
    [SerializeField, Range(1, 5)]
    private int hardRoomInterval = 4;

    [Header("State Info")]
    [SerializeField]
    private Room prevRoom;
    [SerializeField]
    private Room curRoom;
    [SerializeField]
    private Room nextRoom;

    public int CurRoomIndex { get; private set; }

    #region Events
    private void OnEnable()
    {
        RoomEdge.OnRoomComplete += HandleRoomComplete;
    }

    private void OnDisable()
    {
        RoomEdge.OnRoomComplete -= HandleRoomComplete;
    }
    #endregion

    // Initializes the singleton
    public void InitSingleton()
    {

    }

    // Clears the spawn data of the manager
    public void ClearSpawnData()
    {
        prevRoom = null;
        curRoom = null;
        nextRoom = null;
    }

    #region Room Spawning
    public void GenerateFirstRoom()
    {
        prevRoom = null;
        if (curRoom == null)
        {
            var rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length != 0)
                curRoom = rooms[0].GetComponent<Room>();
            else
                curRoom = SpawnRoom(0, Vector3.zero);
        }
        nextRoom = SpawnRoom(1, curRoom.GetNextRoomPos());
    }

    // Handles completing the current room, destroying previous the previous room and spawning the next one
    private void HandleRoomComplete(Room.RoomType roomType)
    {
        CurRoomIndex++;

        // Destroy previous room
        if (prevRoom != null)
            Destroy(prevRoom.gameObject);

        // Set new rooms
        prevRoom = curRoom;
        curRoom = nextRoom;
        nextRoom = SpawnRoom(CurRoomIndex, curRoom.GetNextRoomPos());
    }

    // Spawns a room at the given index
    private Room SpawnRoom(int targetIndex, Vector3 targetPos)
    {
        if (targetIndex != 0 && targetIndex % hardRoomInterval == 0)
            return HandleRoomSpawn(roomList.GetRandomHardRoom(), targetPos, targetIndex, Room.RoomType.HARD);
        else
            return HandleRoomSpawn(roomList.GetRandomEasyRoom(), targetPos, targetIndex, Room.RoomType.EASY);
    }

    // Spawns the given room at the given position, initializing it
    private Room HandleRoomSpawn(GameObject room, Vector3 newRoomPos, int _index, Room.RoomType _type)
    {
        Room tempRoom = Instantiate(room, newRoomPos, Quaternion.identity, PrefabManager.Instance.levelHolder).GetComponent<Room>();
        if (tempRoom == null)
            return null;

        tempRoom.InitRoom(_type, _index);
        return tempRoom;
    }
    #endregion

    #region Content Spawning
    public void SpawnContent(GlobalVars.ContentType type, Vector3 pos)
    {
        HandleSpawnContent(contentList.GetRandomContent(type), pos);
    }

    // Spawns the given content at the given position
    private void HandleSpawnContent(GameObject content, Vector3 targetPos)
    {
        Instantiate(content, targetPos, Quaternion.identity, PrefabManager.Instance.contentHolder);
    }
    #endregion

    #region Difficulty
    public float GetCameraMoveMod()
    {
        return 1.0f + diffMods.cameraSpeedUpMod * CurRoomIndex;
    }
    #endregion
}