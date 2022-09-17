using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyMods", menuName = "ScriptableObjects/DifficultyMods", order = 1)]
[System.Serializable]
public class DifficultyMods : ScriptableObject
{
    [Range(0.1f, 10.0f)]
    public float cameraSpeedUpMod = 0.1f;
}
