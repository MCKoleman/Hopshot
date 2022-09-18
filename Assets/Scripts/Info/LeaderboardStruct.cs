
[System.Serializable]
public struct LeaderboardStruct
{
    public int rank;
    public string username;
    public int score;

    public LeaderboardStruct(int _rank, string _username, int _score)
    {
        rank = _rank;
        username = _username;
        score = _score;
    }
}
