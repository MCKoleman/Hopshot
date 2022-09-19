using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIHUD : UIComponent
{
    [SerializeField]
    private TextMeshProUGUI highscoreText;
    [SerializeField]
    private TextMeshProUGUI curScoreText;
    [SerializeField]
    private UIBoopCooldown boopCooldown;
    [SerializeField]
    private float tweenDuration = 0.4f;

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdate += UpdateScore;
        ScoreManager.OnHighscoreUpdate += UpdateHighscore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdate -= UpdateScore;
        ScoreManager.OnHighscoreUpdate -= UpdateHighscore;
    }

    // Updates the displayed scores whenever the HUD is enabled
    public override void Enable()
    {
        base.Enable();
        UpdateScore(ScoreManager.Instance.CurScore);
        UpdateHighscore(ScoreManager.Instance.Highscore);
    }

    // Updates the boop cooldown display
    public void UpdateBoopCooldown(float percent) { boopCooldown.SetSliderFill(percent); }

    // Updates the score displayed on screen
    public void UpdateScore(int score)
    {
        UpdateText(curScoreText, score.ToString());
    }

    // Sets the highscore of the UI
    public void UpdateHighscore(int score)
    {
        UpdateText(highscoreText, score.ToString());
    }

    // Updates text, animating it with a bounce
    public void UpdateText(TextMeshProUGUI text, string value)
    {
        text.text = value;
        text.transform.DOScale(1.2f, tweenDuration)
            .OnComplete(() => text.transform.DOScale(1.0f, tweenDuration));
    }
}
