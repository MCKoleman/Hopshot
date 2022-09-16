using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject uiElement;
    public bool IsEnabled { get; private set; }

    public void Enable()
    {
        uiElement.SetActive(true);
        IsEnabled = true;
    }

    public void Disable()
    {
        uiElement.SetActive(false);
        IsEnabled = false;
    }
}
