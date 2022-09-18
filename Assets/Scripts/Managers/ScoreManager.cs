using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    // Inspector variables
    [SerializeField]
    private ScoreList scoreList;

    // Variables
    private int m_curScore = 0;
    private int m_highscore = 0;
    public int CurScore { get { return m_curScore; } private set { m_curScore = value; OnScoreUpdate?.Invoke(m_curScore); } }
    public int Highscore { get { return m_highscore; } private set { m_highscore = value; OnHighscoreUpdate?.Invoke(m_curScore); } }

    #region Events
    public delegate void ScoreUpdate(int score);
    public static event ScoreUpdate OnScoreUpdate;

    public delegate void HighscoreUpdate(int score);
    public static event HighscoreUpdate OnHighscoreUpdate;

    private void OnEnable()
    {
        RoomEdge.OnRoomComplete += HandleRoomComplete;
    }

    private void OnDisable()
    {
        RoomEdge.OnRoomComplete -= HandleRoomComplete;
    }
    #endregion

    public void InitSingleton()
    {
        CurScore = 0;
        Highscore = 0;
    }

    public void InitHighscore()
    {
        CurScore = 0;
        Highscore = LootLockerManager.Instance.GetUserHighscore();
    }

    public void SubmitScore()
    {
        LootLockerManager.Instance.SubmitScore(CurScore);
        CurScore = 0;

        /*
#if UNITY_EDITOR
        LootLockerManager.Instance.DEBUG_SubmitScore(5, "1234560", "Debug1");
        LootLockerManager.Instance.DEBUG_SubmitScore(5, "1234561", "Debug2");
        LootLockerManager.Instance.DEBUG_SubmitScore(10, "1234562", "Debug3");
        LootLockerManager.Instance.DEBUG_SubmitScore(15, "1234563", "Debug4");
        LootLockerManager.Instance.DEBUG_SubmitScore(20, "1234564", "Debug5");
        LootLockerManager.Instance.DEBUG_SubmitScore(25, "1234565", "Debug6");
        LootLockerManager.Instance.DEBUG_SubmitScore(30, "1234566", "Debug7");
        LootLockerManager.Instance.DEBUG_SubmitScore(35, "1234567", "Debug8");
        LootLockerManager.Instance.DEBUG_SubmitScore(40, "1234568", "Debug9");
        LootLockerManager.Instance.DEBUG_SubmitScore(45, "1234569", "Debug10");
        LootLockerManager.Instance.DEBUG_SubmitScore(50, "1234570", "Debug11");
#endif*/
    }

    // Handles completing a room
    private void HandleRoomComplete(Room.RoomType roomType)
    {
        switch(roomType)
        {
            case Room.RoomType.EASY:
                AddScore(scoreList.easyRoomScore);
                break;
            case Room.RoomType.HARD:
                AddScore(scoreList.hardRoomScore);
                break;
            default:
                break;
        }
    }

    // Adds the given score to the current score
    public void AddScore(int score)
    {
        CurScore += score;
        if (CurScore > Highscore)
            Highscore = CurScore;
    }
}
