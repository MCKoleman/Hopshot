using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreList", menuName = "ScriptableObjects/ScoreList", order = 1)]
[System.Serializable]
public class ScoreList : ScriptableObject
{
    public int enemyKillScore = 1;
    public int easyRoomScore = 5;
    public int hardRoomScore = 10;
}
