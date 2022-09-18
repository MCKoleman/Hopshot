using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private bool DEBUG_DISABLE_GENERATION = true;
    [SerializeField]
    private bool DEBUG_SIMULATE_MOBILE = false;

    public bool IsGameActive { get; private set; }
    public bool IsMobile { get; private set; }

    public delegate void MobileStatusChanged(bool isMobile);
    public event MobileStatusChanged OnMobileStatusChange;
    public delegate void GameStart();
    public static event GameStart OnGameStart;
    public delegate void GameEnd();
    public static event GameEnd OnGameEnd;

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
        PrefabManager.Instance.InitSingleton();
        UIManager.Instance.InitSingleton();
        ScoreManager.Instance.InitSingleton();
        SpawnManager.Instance.InitSingleton();
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
        switch(sceneType)
        {
            case SceneDetector.SceneType.GAME:
                UIManager.Instance.EnableHUD();

                // Start game
                StartGame();
                break;
            case SceneDetector.SceneType.MAIN_MENU:
                UIManager.Instance.EnableMainMenu();
                break;
            default:
                break;
        }
    }

    public bool IsGamePaused() { return Time.timeScale == 0.0f; }
}
