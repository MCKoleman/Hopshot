using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    // Inspector variables
    [SerializeField]
    private ScoreList scoreList;

    // Variables
    [SerializeField]
    private int m_curScore = 0;
    [SerializeField]
    private int m_highscore = 0;
    public int CurScore { get { return m_curScore; } private set { m_curScore = value; OnScoreUpdate?.Invoke(m_curScore); } }
    public int Highscore 
    { 
        get
        {
            return m_highscore;
        } 
        private set 
        {
            if(value > m_highscore)
                m_highscore = value;
            OnHighscoreUpdate?.Invoke(m_highscore);
        } 
    }

    #region Events
    public delegate void ScoreUpdate(int score);
    public static event ScoreUpdate OnScoreUpdate;

    public delegate void HighscoreUpdate(int score);
    public static event HighscoreUpdate OnHighscoreUpdate;

    private void OnEnable()
    {
        LootLockerManager.OnLoadHighscore += HandleLoadHighscore;
        RoomEdge.OnRoomComplete += HandleRoomComplete;
    }

    private void OnDisable()
    {
        LootLockerManager.OnLoadHighscore -= HandleLoadHighscore;
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
        Highscore = LootLockerManager.Instance.GetUserHighscore();
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

    // Handles loading the highscore from database
    private void HandleLoadHighscore(int _highscore)
    {
        // Only update highscore if it is higher than the current one
        if(_highscore > Highscore)
        {
            Debug.Log($"[ScoreManager] Loaded highscore from database! Old: [{Highscore}], new [{_highscore}]");
            Highscore = _highscore;
        }
    }

    // Adds the given score to the current score
    public void AddScore(int score)
    {
        CurScore += score;
        if (CurScore > Highscore)
        {
            Debug.Log($"[ScoreManager] Reached new highscore! Old: [{Highscore}], new [{CurScore}]");
            Highscore = CurScore;
        }
    }
}
