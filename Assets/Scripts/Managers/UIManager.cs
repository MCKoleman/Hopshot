using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public bool IsPaused { get; private set; }
    [SerializeField]
    private UIHUD hud;
    [SerializeField]
    private UIPauseMenu pauseMenu;
    [SerializeField]
    private UIDeathMenu deathMenu;
    [SerializeField]
    private UIMainMenu mainMenu;
    [SerializeField]
    private UISceneTransition sceneTransition;

    // Initializes the singleton
    public void InitSingleton()
    {
        UnpauseGame();
    }

    // Enables the HUD
    public void EnableHUD()
    {
        hud.Enable();
        pauseMenu.Disable();
        deathMenu.Disable();
        mainMenu.Disable();
    }

    // Enables the main menu
    public void EnableMainMenu()
    {
        hud.Disable();
        deathMenu.Disable();
        pauseMenu.Disable();
        mainMenu.Enable();
    }

    // Enables the death menu
    public void EnableDeathMenu()
    {
        hud.Disable();
        deathMenu.Enable();
        pauseMenu.Disable();
        mainMenu.Disable();
    }

    // Returns to the main menu
    public void ReturnToMainMenu()
    {
        PrefabManager.Instance.ClearContent();
        GameManager.Instance.EndGame();
    }

    #region Transitions
    // Starts the scene fade out transition
    public void SceneFadeOut()
    {
        sceneTransition.FadeOut();
    }

    // Starts the scene fade in transition
    public void SceneFadeIn()
    {
        sceneTransition.FadeIn();
    }
    #endregion

    #region Pausing
    // Toggles the paused state of the game
    public void TogglePause()
    {
        if (IsPaused)
            UnpauseGame();
        else
            PauseGame();
    }

    // Pauses the game
    public void PauseGame()
    {
        pauseMenu.Enable();
        Time.timeScale = 0.0f;
        IsPaused = true;
    }

    // Unpauses the game
    public void UnpauseGame()
    {
        pauseMenu.Disable();
        Time.timeScale = 1.0f;
        IsPaused = false;
    }
    #endregion
}
