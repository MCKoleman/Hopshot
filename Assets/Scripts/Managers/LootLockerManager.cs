using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LootLocker.Requests;

public class LootLockerManager : Singleton<LootLockerManager>
{
    private string memberID = "";
    private string playerName = "";
    private int leaderboardID = 7136;

    public void InitSingleton()
    {
        InitLootLocker();
    }

    // Initializes LootLocker and signs in
    public void InitLootLocker()
    {
        /*
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
        });*/
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
        /*
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
        });*/
    }

    // Returns the player's highscore if one exists
    public int GetUserHighscore()
    {
        int trackedScore = 0;

        // Don't return a score for a non-existant user
        if (memberID == "")
            return 0;

        /*
        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log($"[LootLocker] Successfully retrieved player {memberID} score");
                trackedScore = response.score;
                if (response.score != 0 && playerName != response.metadata)
                    playerName = response.metadata;
            }
            else
            {
                Debug.Log($"[LootLocker] Failed to retrieve player {memberID} score: {response.Error}");
            }
        });*/
        return trackedScore;
    }
}
