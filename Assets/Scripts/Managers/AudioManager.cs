using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioClip uiSubmit;
    [SerializeField]
    private AudioClip uiHighlight;
    [SerializeField]
    private AudioClip uiPauseOpen;
    [SerializeField]
    private AudioClip uiPauseClose;
    [SerializeField]
    private AudioClip uiScoreIncrease;
    [SerializeField]
    private AudioClip uiHighscore;
    [SerializeField]
    private AudioClip uiNewHighscore;

    [SerializeField]
    private AudioSource uiSource;

    public void InitSingleton()
    {

    }

    public void UISubmit() { PlayClip(uiHighlight, uiSource); }
    public void UIHighlight() { PlayClip(uiSubmit, uiSource); }
    public void UIPauseOpen() { PlayClip(uiPauseOpen, uiSource); }
    public void UIPauseClose() { PlayClip(uiPauseClose, uiSource); }
    public void UILoopScore() { uiSource.loop = true; PlayClip(uiScoreIncrease, uiSource); }
    public void UIHighscore() { uiSource.loop = false; PlayClip(uiHighscore, uiSource); }
    public void UINewHighscore() { uiSource.loop = false; PlayClip(uiNewHighscore, uiSource); }

    // Plays the given clip in the given source
    private void PlayClip(AudioClip clip, AudioSource src)
    {
        src.clip = clip;
        src.Play();
    }
}
