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

    protected override void EnableCharacter()
    {
        base.EnableCharacter();
        foreach (Transform temp in this.transform)
            temp.gameObject.SetActive(true);
    }

    protected override void DisableCharacter()
    {
        base.DisableCharacter();
        foreach (Transform temp in this.transform)
            temp.gameObject.SetActive(false);
    }
}
