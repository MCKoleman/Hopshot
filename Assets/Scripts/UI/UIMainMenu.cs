using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIComponent
{
    [SerializeField]
    private GameObject loginScreen;
    [SerializeField]
    private UILeaderboard leaderboard;

    public override void Enable()
    {
        base.Enable();
        loginScreen.SetActive(!LootLockerManager.Instance.GetIsLoggedIn() || !LootLockerManager.Instance.HasName());
    }

    public void SubmitUsername(string name)
    {
        LootLockerManager.Instance.SetUsername(name.ToUpper());
    }
}
