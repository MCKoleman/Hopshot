using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIDeathMenu : UIComponent
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI highscoreText;
    [SerializeField]
    private float scoreLerpSpeed = 0.0f;

    public override void Enable()
    {
        base.Enable();
        ShowScores();
    }

    public void ShowScores()
    {

    }

    private IEnumerator CycleToScore(int highscore, int curScore)
    {
        yield return new WaitForSeconds(0.0f);
    }
}
