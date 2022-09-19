using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool isFadeOutComplete = false;
    private bool isFadeInComplete = false;

    #region Scene Transition Listeners
    private void OnEnable()
    {
        UISceneTransition.OnFadeInComplete += HandleFadeInComplete;
        UISceneTransition.OnFadeOutComplete += HandleFadeOutComplete;
    }

    private void OnDisable()
    {
        UISceneTransition.OnFadeInComplete -= HandleFadeInComplete;
        UISceneTransition.OnFadeOutComplete -= HandleFadeOutComplete;
    }

    private void HandleFadeOutComplete() { isFadeOutComplete = true; }
    private void HandleFadeInComplete() { isFadeInComplete = true; }
    #endregion

    // Loads the scene with the given ID instantly
    public void LoadSceneWithId(int level)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(level);
    }

    // Asynchronously loads the scene, using scene transitions along the way
    public void LoadSceneAsync(int level)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(HandleAsyncSceneLoad(level));
    }

    // Handles loading a scene in the background
    private IEnumerator HandleAsyncSceneLoad(int level)
    {
        isFadeInComplete = false;
        isFadeOutComplete = false;

        // Fades out the screen
        UIManager.Instance.SceneFadeOut();
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.UILoadScene();
        yield return new WaitUntil(() => isFadeOutComplete);

        // Loads scene in background
        AsyncOperation loading = SceneManager.LoadSceneAsync(level);
        GameManager.Instance.HandleSceneStartLoad(level);
        yield return new WaitUntil(() => loading.isDone);

        // Fades in the screen
        UIManager.Instance.SceneFadeIn();
        yield return new WaitUntil(() => isFadeInComplete);

        // Activates the game
        GameManager.Instance.HandleSceneLoad();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}