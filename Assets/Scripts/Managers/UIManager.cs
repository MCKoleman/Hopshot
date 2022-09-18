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

    public void InitSingleton()
    {
        UnpauseGame();
    }

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
