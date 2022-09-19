using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerManager : Singleton<LootLockerManager>
{
    public delegate void LoadHighscore(int highscore);
    public static event LoadHighscore OnLoadHighscore;
    public delegate void SignIn(string username);
    public static event SignIn OnSignIn;

    private string memberID = "";
    private string playerName = "";
    private bool isLoggedIn = false;

    private bool isLoggingIn = false;
    private const int leaderboardID = 7206;

    public void InitSingleton()
    {
        isLoggedIn = false;
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
            isLoggingIn = true;
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

        string tempName = (playerName != "") ? playerName : "ANON BOT";
        LootLockerSDKManager.SubmitScore(memberID, score, leaderboardID, tempName, (response) =>
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
    public void GetUserHighscore()
    {
        // Don't return a score for a non-existant user
        if (memberID == "")
            return;

        
        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log($"[LootLocker] Successfully retrieved player {memberID} score");
                if (response.score != 0 && playerName != response.metadata)
                    playerName = response.metadata;

                OnLoadHighscore?.Invoke(response.score);

                // When requesting a score while logging in, signal a successful signin
                if(isLoggingIn)
                {
                    OnSignIn?.Invoke(playerName);
                    isLoggingIn = false;
                }
            }
            else
            {
                Debug.Log($"[LootLocker] Failed to retrieve player {memberID} score: {response.Error}");
            }
        });
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
        // Start searching from previous results all the way through current and next results, storing the latest five
        int startIndex = (index <= 0) ? 0 : (index - UILeaderboard.LEADERBOARD_DISPLAY_COUNT);
        int searchCount = ((index <= 0) ? 2 : 3) * UILeaderboard.LEADERBOARD_DISPLAY_COUNT;

        LootLockerSDKManager.GetScoreList(leaderboardID, searchCount, startIndex, (response) =>
        {
            LootLockerLeaderboardMember[] values = response.items;
            results.GetLeaderboardResultsFromLootLockerResults(ref values);
            results.Trim(UILeaderboard.LEADERBOARD_DISPLAY_COUNT, true);
            UILeaderboard.AsyncUpdateLeaderboardTrigger(Mathf.Max(results.GetLastRank() - UILeaderboard.LEADERBOARD_DISPLAY_COUNT - 1, 0));

            /*
            LootLockerSDKManager.GetNextScoreList(leaderboardID, UILeaderboard.LEADERBOARD_DISPLAY_COUNT, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log($"[LootLocker] Successfully retrieved top scores after index {index}");
                    LootLockerLeaderboardMember[] values = response.items;
                    results.AppendLeaderboardResultsFromLootLockerResults(ref values);
                }
                else
                {
                    Debug.Log($"[LootLocker] Failed to retrieve top scores after {index}");
                }
            });*/
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

                int rankIndex = response.rank - 1;
                int halfDisplay = Mathf.FloorToInt(UILeaderboard.LEADERBOARD_DISPLAY_COUNT * 0.5f);
                int after = rankIndex < halfDisplay + 1 ? 0 : rankIndex - halfDisplay;

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
