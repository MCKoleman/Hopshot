using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    protected override void HandleDeath()
    {
        base.HandleDeath();
        GameManager.Instance.HandlePlayerDeath();
    }
}
