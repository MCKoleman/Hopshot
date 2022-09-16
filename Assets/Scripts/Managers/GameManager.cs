using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameActive { get; private set; }

    private void Start()
    {
        // Start game
        // Init everything
        UIManager.Instance.InitSingleton();
        this.InitSingleton();
        StartGame();
    }

    public void InitSingleton()
    {
        // 
    }

    public void StartGame()
    {
        IsGameActive = true;
    }

    public void EndGame()
    {
        IsGameActive = false;
    }

    public bool IsGamePaused() { return Time.timeScale == 0.0f; }
}
