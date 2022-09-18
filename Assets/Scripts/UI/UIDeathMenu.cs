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
    private TextMeshProUGUI newHighscoreText;
    [SerializeField]
    private GameObject oldHighscoreHolder;
    [SerializeField]
    private GameObject newHighscoreHolder;
    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private float scoreLerpTime = 0.0f;
    [SerializeField]
    private float scoreBounceTime = 0.2f;
    [SerializeField]
    private float newHighscoreTime = 0.05f;

    public override void Enable()
    {
        base.Enable();
        ShowScores();
    }

    // Displays the players scores in the UI
    public void ShowScores()
    {
        scoreText.text = "0";

        oldHighscoreHolder.SetActive(false);
        newHighscoreHolder.SetActive(false);
        buttons.SetActive(false);

        StartCoroutine(CycleToScore(ScoreManager.Instance.Highscore, ScoreManager.Instance.CurScore));
    }

    // Cycles the player's score display to their final score
    private IEnumerator CycleToScore(int highscore, int curScore)
    {
        // Enable first score object
        scoreText.gameObject.SetActive(true);

        // Lerp score to final score
        float curLerpTime = 0.0f;
        float tempScore = 0.0f;
        while(Mathf.RoundToInt(tempScore) < curScore)
        {
            // Handle timer
            yield return new WaitForEndOfFrame();
            curLerpTime += Time.deltaTime;
            if (curLerpTime > scoreLerpTime)
                curLerpTime = scoreLerpTime;

            // Lerp score
            float percent = curLerpTime / scoreLerpTime;
            percent = Mathf.Sin(percent * 0.5f * Mathf.PI);
            tempScore = Mathf.Lerp(0, curScore, percent);

            // Display temp score
            scoreText.text = Mathf.FloorToInt(tempScore).ToString("D9");
        }
        // Display final score
        scoreText.text = curScore.ToString("D9");
        scoreText.transform.DOScale(1.2f, scoreBounceTime)
            .OnComplete(() => scoreText.transform.DOScale(1.0f, scoreBounceTime));

        // If a new highscore was set, set the new highscore display
        if (curScore >= highscore)
        {
            newHighscoreText.text = "";
            newHighscoreHolder.SetActive(true);
            StartCoroutine(CycleNewHighscoreText());
        }
        else
        {
            highscoreText.text = highscore.ToString("D9");
            oldHighscoreHolder.SetActive(true);
            highscoreText.transform.DOScale(1.2f, scoreBounceTime)
                .OnComplete(() => highscoreText.transform.DOScale(1.0f, scoreBounceTime));
        }

        // Enable buttons when all animations are done
        buttons.SetActive(true);
    }

    // Cycles the highscore to its target text
    private IEnumerator CycleNewHighscoreText()
    {
        string targetText = "New highscore";
        string curText = "";
        int stealIndex = 0;
        while(curText != targetText)
        {
            // Fills curText with the target characters, one by one
            yield return new WaitForSecondsRealtime(newHighscoreTime);
            curText += targetText[stealIndex];
            stealIndex++;
            newHighscoreText.text = curText;
        }
        // Bounce animation on complete
        newHighscoreHolder.transform.DOScale(1.2f, scoreBounceTime)
            .OnComplete(() => newHighscoreHolder.transform.DOScale(1.0f, scoreBounceTime));
    }
}
