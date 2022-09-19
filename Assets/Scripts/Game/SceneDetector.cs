using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetector : MonoBehaviour
{
    public delegate void SceneStart(SceneType sceneType);
    public static SceneStart OnSceneStart;
    public enum SceneType { DEFAULT, MAIN_MENU, GAME }
    [SerializeField]
    private SceneType sceneType;

    private void OnEnable()
    {
        GameManager.OnSceneLoad += SignalSceneType;
    }

    private void OnDisable()
    {
        GameManager.OnSceneLoad -= SignalSceneType;
    }

    private void SignalSceneType()
    {
        OnSceneStart?.Invoke(sceneType);
    }
}