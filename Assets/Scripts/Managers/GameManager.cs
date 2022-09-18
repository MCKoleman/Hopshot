using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private bool DEBUG_DISABLE_GENERATION = true;
    public bool IsGameActive { get; private set; }

    private void Start()
    {
        // Start game
        // Init everything
        PrefabManager.Instance.InitSingleton();
        UIManager.Instance.InitSingleton();
        SpawnManager.Instance.InitSingleton();
        this.InitSingleton();
        StartGame();
    }

    public void InitSingleton()
    {
        // 
    }

    // Starts the game, generating the level
    public void StartGame()
    {
        IsGameActive = true;
#if UNITY_EDITOR
        if(!DEBUG_DISABLE_GENERATION)
#endif
        SpawnManager.Instance.GenerateFirstRoom();
    }

    // Ends the game, disabling the level
    public void EndGame()
    {
        IsGameActive = false;
        PrefabManager.Instance.ClearContent();
    }

    public bool IsGamePaused() { return Time.timeScale == 0.0f; }
}
