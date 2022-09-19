using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneTransition : MonoBehaviour
{
    private bool isFaded = false;
    private Animator anim;

    public delegate void FadeOutComplete();
    public delegate void FadeInComplete();
    public static event FadeOutComplete OnFadeOutComplete;
    public static event FadeInComplete OnFadeInComplete;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        SetFaded(false);
    }

    // Signals the completion of the fade in animation
    public void HandleFadeInComplete()
    {
        Debug.Log("[SceneTransition] Fade in complete");
        OnFadeInComplete?.Invoke();
    }

    // Signals the completion of the fade out animation
    public void HandleFadeOutComplete()
    {
        Debug.Log("[SceneTransition] Fade out complete");
        OnFadeOutComplete?.Invoke();
    }

    // Animates the fade out sequence
    public void FadeOut()
    {
        Debug.Log("[SceneTransition] Fading out...");
        SetFaded(true);
    }

    // Animates the fade in sequence
    public void FadeIn()
    {
        Debug.Log("[SceneTransition] Fading in...");
        SetFaded(false);
    }

    private void SetFaded(bool value)
    {
        isFaded = value;
        anim?.SetBool("isFaded", value);
    }
}