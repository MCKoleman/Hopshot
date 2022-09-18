using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

[System.Serializable]
public class LeaderboardResults
{
    [SerializeField]
    private List<LeaderboardStruct> results;

    public LeaderboardResults()
    {
        results = new List<LeaderboardStruct>();
    }

    // Returns the struct at the given index
    public LeaderboardStruct Get(int index)
    {
        return results[index];
    }

    // Returns the last rank from the list
    public int GetLastRank()
    {
        if (results.Count < 0)
            return -1;
        return results[results.Count - 1].rank;
    }

    // Fills the results list until it has num elements
    public void Fill(int elementCount)
    {
        while (results.Count < elementCount)
            results.Add(new LeaderboardStruct(-1, "", -1));
    }

    // Gets leaderboard results from the given LootLocker results
    public void GetLeaderboardResultsFromLootLockerResults(ref LootLockerLeaderboardMember[] values)
    {
        results.Clear();
        AppendLeaderboardResultsFromLootLockerResults(ref values);
    }

    // Appends LootLocker results to the leaderboard results
    public void AppendLeaderboardResultsFromLootLockerResults(ref LootLockerLeaderboardMember[] values)
    {
        foreach (LootLockerLeaderboardMember member in values)
            results.Add(new LeaderboardStruct(member.rank, member.metadata, member.score));
    }
}
