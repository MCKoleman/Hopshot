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
