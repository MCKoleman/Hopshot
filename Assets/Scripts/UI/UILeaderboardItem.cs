using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILeaderboardItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Updates the info of the component
    public void UpdateInfo(int rank, string name, int score)
    {
        // Only show rankText or scoreText if their values are greater than 0
        rankText.text = (rank > 0) ? ("#" + rank.ToString()) : "";
        scoreText.text = (score > 0) ? (score.ToString()) : "";
        nameText.text = name;
    }

    // Updates the info of the component directly from a struct
    public void UpdateInfo(LeaderboardStruct info) { UpdateInfo(info.rank, info.username, info.score); }
}
