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
    private AudioClip uiLoadScene;

    [SerializeField]
    private AudioClip roomComplete;

    [SerializeField]
    private List<AudioClip> menuMusicList;
    [SerializeField]
    private List<AudioClip> gameMusicList;

    [SerializeField]
    private AudioSource uiSource;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource gameSource;
    private bool isInGame = false;

    private void OnEnable()
    {
        RoomEdge.OnRoomComplete += RoomComplete;
    }

    private void OnDisable()
    {
        RoomEdge.OnRoomComplete -= RoomComplete;
    }

    public void InitSingleton()
    {
        isInGame = false;
    }

    // Make sure appropriate music is playing at all times
    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            if(isInGame)
                PlayClip(gameMusicList[Random.Range(0, gameMusicList.Count)], musicSource);
            else
                PlayClip(menuMusicList[Random.Range(0, menuMusicList.Count)], musicSource);
        }
    }

    public void SetIsInGame(bool value)
    {
        isInGame = value;
        if (isInGame)
            PlayClip(gameMusicList[Random.Range(0, gameMusicList.Count)], musicSource);
        else
            PlayClip(menuMusicList[Random.Range(0, menuMusicList.Count)], musicSource);
    }

    public void RoomComplete(Room.RoomType ignored) { PlayClip(roomComplete, gameSource); }

    public void UISubmit() { PlayClip(uiHighlight, uiSource); }
    public void UIHighlight() { PlayClip(uiSubmit, uiSource); }
    public void UIPauseOpen() { PlayClip(uiPauseOpen, uiSource); }
    public void UIPauseClose() { PlayClip(uiPauseClose, uiSource); }
    public void UILoopScore() { uiSource.loop = true; PlayClip(uiScoreIncrease, uiSource); }
    public void UIHighscore() { uiSource.loop = false; PlayClip(uiHighscore, uiSource); }
    public void UINewHighscore() { uiSource.loop = false; PlayClip(uiNewHighscore, uiSource); }
    public void UILoadScene() { PlayClip(uiLoadScene, uiSource); }

    // Plays the given clip in the given source
    private void PlayClip(AudioClip clip, AudioSource src)
    {
        src.clip = clip;
        src.Play();
    }
}
