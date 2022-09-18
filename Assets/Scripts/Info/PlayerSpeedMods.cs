using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpeedMods", menuName = "ScriptableObjects/PlayerSpeedMods", order = 1)]
[System.Serializable]
public class PlayerSpeedMods : ScriptableObject
{
    [Header("Movement mods")]
    public float MOVE_SPEED = 5.0f;     // Movement speed
    public float MOVE_ACC_MOD = 5.0f;   // Movement acceleration modifier
    public float MOVE_DEC_MOD = 5.0f;   // Movement deceleration modifier
    public float AIR_ACC_MOD = 0.5f;    // Air movement acceleration modifier
    public float AIR_DEC_MOD = 0.5f;    // Air movement deceleration modifier

    [Header("Jump mods")]
    public float JUMP_FORCE = 5.0f;     // Jump force
    public float GRAVITY_NORMAL = 1.0f; // Gravity when moving around/up
    public float GRAVITY_HEAVY = 3.0f;  // Gravity when falling
}
