using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    protected List<Transform> disableOnDeath = new List<Transform>();

    protected override void HandleDeath()
    {
        base.HandleDeath();
        GameManager.Instance.HandlePlayerDeath();
    }

    protected override void DestroySelf()
    {
        DisableCharacter();
    }

    protected override void EnableCharacter()
    {
        base.EnableCharacter();
        foreach (Transform temp in disableOnDeath)
            temp.gameObject.SetActive(true);
    }

    protected override void DisableCharacter()
    {
        base.DisableCharacter();
        foreach (Transform temp in disableOnDeath)
            temp.gameObject.SetActive(false);
    }
}
