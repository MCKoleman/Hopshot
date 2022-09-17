using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate_Logic : MonoBehaviour
{
    [SerializeField]
    private Gate_Button button;

    [SerializeField]
    private Sprite[] pics;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        button.OnButtonPress += Open;
        button.OnButtonUnpress += Close;
    }

    private void OnDisable()
    {
        button.OnButtonPress -= Open;
        button.OnButtonUnpress -= Close;
    }

    private void Open()
    {
        rb.simulated = false;
        spriteRenderer.sprite = pics[1];
    }

    private void Close() 
    {
        rb.simulated = true;
        spriteRenderer.sprite = pics[0];
    }
}
