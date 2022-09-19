using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerManager : Singleton<LootLockerManager>
{
    public delegate void LoadHighscore(int highscore);
    public static event LoadHighscore OnLoadHighscore;

    private string memberID = "";
    private string playerName = "";
    private bool isLoggedIn = false;

    private const int leaderboardID = 7136;

    public void InitSingleton()
    {
        InitLootLocker();
    }

    // Initializes LootLocker and signs in
    public void InitLootLocker()
    {
        
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("[LootLocker]: Error starting LootLocker session");

                return;
            }
            Debug.Log("[LootLocker]: Successfully started LootLocker session");

            // Store player ID
            memberID = response.player_id.ToString();
            GetUserHighscore();
            isLoggedIn = true;
        });
    }

    // Sets the player's username
    public void SetUsername(string name)
    {
        playerName = name;
    }

    // Submits the player's score to leaderboard
    public void SubmitScore(int score)
    {
        // Don't submit negative or zero score
        if (score <= 0)
            return;

        // Only submit a new score if it is higher than the previous one
        if (GetUserHighscore() >= score)
            return;
        
        LootLockerSDKManager.SubmitScore(memberID, score, leaderboardID, playerName, (response) =>
        {
            if(response.statusCode == 200)
            {
                Debug.Log("[LootLocker]: Successfully submitted score");
            }
            else
            {
                Debug.Log("[LootLocker]: Failed to submit score");
            }
        });
    }

#if UNITY_EDITOR
    // Submits a debug score
    public void DEBUG_SubmitScore(int score, string _memberID, string _playerName)
    {
        LootLockerSDKManager.SubmitScore(_memberID, score, leaderboardID, _playerName, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("[LootLocker]: Successfully submitted score");
            }
            else
            {
                Debug.Log("[LootLocker]: Failed to submit score");
            }
        });
    }
#endif

    // Returns the player's highscore if one exists
    public int GetUserHighscore()
    {
        int trackedScore = 0;

        // Don't return a score for a non-existant user
        if (memberID == "")
            return 0;

        
        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log($"[LootLocker] Successfully retrieved player {memberID} score");
                trackedScore = response.score;
                if (response.score != 0 && playerName != response.metadata)
                    playerName = response.metadata;

                OnLoadHighscore?.Invoke(response.score);
            }
            else
            {
                Debug.Log($"[LootLocker] Failed to retrieve player {memberID} score: {response.Error}");
            }
        });
        return trackedScore;
    }

    // Returns the top LEADERBOARD_DISPLAY_COUNT results from the leaderboard at the given index
    public void GetLeaderboardHighscores(LeaderboardResults results, int index)
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, UILeaderboard.LEADERBOARD_DISPLAY_COUNT, index, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log($"[LootLocker] Successfully retrieved scores around index {index}");
                LootLockerLeaderboardMember[] values = response.items;
                results.GetLeaderboardResultsFromLootLockerResults(ref values);
                UILeaderboard.AsyncUpdateLeaderboardTrigger(index);
            }
            else
            {
                Debug.Log($"[LootLocker] Failed to retrieve top scores around index {index}");
            }
        });
    }

    // Returns the next LEADERBOARD_DISPLAY_COUNT results from the leaderboard after the given index
    public void GetLeaderboardNextScores(LeaderboardResults results, int index)
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, UILeaderboard.LEADERBOARD_DISPLAY_COUNT, index, (response) =>
        {
            LootLockerLeaderboardMember[] values = response.items;
            results.GetLeaderboardResultsFromLootLockerResults(ref values);

            LootLockerSDKManager.GetNextScoreList(leaderboardID, UILeaderboard.LEADERBOARD_DISPLAY_COUNT, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log($"[LootLocker] Successfully retrieved top scores after index {index}");
                    LootLockerLeaderboardMember[] values = response.items;
                    results.AppendLeaderboardResultsFromLootLockerResults(ref values);
                    UILeaderboard.AsyncUpdateLeaderboardTrigger(results.GetLastRank()-1);
                }
                else
                {
                    Debug.Log($"[LootLocker] Failed to retrieve top scores after {index}");
                }
            });
        });
    }

    // Gets the top scores around the player
    public void GetLeaderboardHighscoresAroundPlayer(LeaderboardResults results)
    {
        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log($"[LootLocker] Successfully retrieved scores around player {memberID}");

                int rank = response.rank;
                int halfDisplay = Mathf.FloorToInt(UILeaderboard.LEADERBOARD_DISPLAY_COUNT * 0.5f);
                int after = rank < halfDisplay + 1 ? 0 : rank - halfDisplay;

                GetLeaderboardHighscores(results, after);
            }
            else
            {
                Debug.Log($"[LootLocker] Failed to retrieve scores around player {memberID}: {response.Error}");
            }
        });
    }

    public bool GetIsLoggedIn() { return isLoggedIn; }
    public bool HasName() { return playerName != ""; }
}
