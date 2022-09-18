using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderboard : MonoBehaviour
{
    [SerializeField]
    private List<UILeaderboardItem> leaderboardSlots;

    [SerializeField]
    private LeaderboardResults results;
    private int curDisplayIndex = -1;

    public const int LEADERBOARD_DISPLAY_COUNT = 5;

    public delegate void UpdateLeaderboard(int curDisplayIndex);
    public static event UpdateLeaderboard OnUpdateLeaderboard;

    private void OnEnable()
    {
        OnUpdateLeaderboard += HandleUpdateLeaderboard;
        GetTopScore();
    }

    private void OnDisable()
    {
        OnUpdateLeaderboard -= HandleUpdateLeaderboard;
    }

    // Gets and displays the top score
    public void GetTopScore()
    {
        LootLockerManager.Instance.GetLeaderboardHighscores(results, 0);
    }

    // Gets and displays scores around your score
    public void GetYourScore()
    {
        LootLockerManager.Instance.GetLeaderboardHighscoresAroundPlayer(results);
    }

    // Gets the next scores moving up
    public void GetNextScoresUp()
    {
        LootLockerManager.Instance.GetLeaderboardHighscores(results, Mathf.Max(curDisplayIndex - LEADERBOARD_DISPLAY_COUNT, 0));
    }

    // Gets the next scores moving down
    public void GetNextScoresDown()
    {
        LootLockerManager.Instance.GetLeaderboardNextScores(results, curDisplayIndex);
    }

    // Triggers the leaderboard update event from outside the class
    public static void AsyncUpdateLeaderboardTrigger(int index) { OnUpdateLeaderboard?.Invoke(index); }

    // Handles updating the leaderboard display when operations are finished
    public void HandleUpdateLeaderboard(int _curDisplayIndex)
    {
        curDisplayIndex = _curDisplayIndex;
        results.Fill(LEADERBOARD_DISPLAY_COUNT);
        DisplayScores();
    }

    // Displays the currently stored scores in the UI
    public void DisplayScores()
    {
        for(int i = 0; i < leaderboardSlots.Count; i++)
            leaderboardSlots[i].UpdateInfo(results.Get(i));
    }
}
