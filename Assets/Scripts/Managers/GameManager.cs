using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool DEBUG_DISABLE_GENERATION = true;
    [SerializeField]
    private bool DEBUG_SIMULATE_MOBILE = false;

    public bool IsGameActive { get; private set; }
    public bool IsMobile { get; private set; }
    public bool IsSceneLoaded { get; private set; }

    public delegate void MobileStatusChanged(bool isMobile);
    public event MobileStatusChanged OnMobileStatusChange;
    public delegate void GameStart();
    public static event GameStart OnGameStart;
    public delegate void GameEnd();
    public static event GameEnd OnGameEnd;
    public delegate void SceneLoad();
    public static event SceneLoad OnSceneLoad;

    #region Events
    private void OnEnable() { SceneDetector.OnSceneStart += HandleSceneSwap; }
    private void OnDisable() { SceneDetector.OnSceneStart -= HandleSceneSwap; }
    #endregion

    private void Start()
    {
        this.InitSingleton();
        SetIsMobile(SystemInfo.deviceType == DeviceType.Handheld);
    }

    public void InitSingleton()
    {
        // Init everything
        LootLockerManager.Instance.InitSingleton();
        PrefabManager.Instance.InitSingleton();
        UIManager.Instance.InitSingleton();
        AudioManager.Instance.InitSingleton();
        ScoreManager.Instance.InitSingleton();
        SpawnManager.Instance.InitSingleton();

        HandleSceneStartLoad(SceneManager.GetActiveScene().buildIndex);
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex != 0)
            HandleSceneLoad();
#endif
    }

    // Starts the game, generating the level
    public void StartGame()
    {
        // Don't start the game if it's already running
        if (IsGameActive)
            return;

        Debug.Log($"[GameManager] Started game");
        OnGameStart?.Invoke();
        IsGameActive = true;
        ScoreManager.Instance.InitHighscore();
    }

    // Handles player death
    public void HandlePlayerDeath()
    {
        EndGame();
        UIManager.Instance.EnableDeathMenu();
    }

    // Ends the game, disabling the level
    public void EndGame()
    {
        // Don't end a game that isn't active
        if (!IsGameActive)
            return;

        Debug.Log($"[GameManager] Ended game");
        OnGameEnd?.Invoke();
        IsGameActive = false;
        ScoreManager.Instance.SubmitScore();
        SpawnManager.Instance.ClearSpawnData();
    }

    // Updates the mobile position of the UI
    public void SetIsMobile(bool _value)
    {
#if UNITY_EDITOR
        IsMobile = _value || DEBUG_SIMULATE_MOBILE;
#else
        IsMobile = _value;
#endif
        OnMobileStatusChange?.Invoke(IsMobile);
    }

    // Handles switching the scene to the given type
    private void HandleSceneSwap(SceneDetector.SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneDetector.SceneType.GAME:
                UIManager.Instance.EnableHUD();
                AudioManager.Instance.SetIsInGame(true);
                StartGame();
                break;
            case SceneDetector.SceneType.MAIN_MENU:
                UIManager.Instance.EnableMainMenu();
                AudioManager.Instance.SetIsInGame(false);
                break;
            default:
                break;
        }
    }

    // Handles generating the level and enabling UI in the background while the level is being loaded
    public void HandleSceneStartLoad(int level)
    {
        // Handle main menu loading
        if (level == 0)
        {
            UIManager.Instance.EnableMainMenu();
            PrefabManager.Instance.ClearContent();
        }
        // Handle regular level loading
        else
        {
            UIManager.Instance.EnableHUD();
            PrefabManager.Instance.ClearContent();

#if UNITY_EDITOR
            if (!DEBUG_DISABLE_GENERATION)
#endif
                SpawnManager.Instance.GenerateFirstRoom();
        }
    }

    // Marks the scene as loaded, signalling it to any listeners
    public void HandleSceneLoad()
    {
        IsSceneLoaded = true;
        OnSceneLoad?.Invoke();
    }

    // Marks the scene as unloaded
    public void HandleSceneUnload()
    {
        IsSceneLoaded = false;
    }

    public bool IsGamePaused() { return Time.timeScale == 0.0f; }
}