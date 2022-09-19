using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLogic : MonoBehaviour
{
    [SerializeField]
    private GateButton button;

    [SerializeField]
    private Sprite[] pics;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private AudioSource source;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        source = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (button == null)
            return;
        button.OnButtonPress += Open;
        button.OnButtonUnpress += Close;
    }

    private void OnDisable()
    {
        if (button == null)
            return;
        button.OnButtonPress -= Open;
        button.OnButtonUnpress -= Close;
    }

    private void Open()
    {
        rb.simulated = false;
        spriteRenderer.sprite = pics[1];
        if(!source.isPlaying)
            source.Play();
    }

    private void Close() 
    {
        rb.simulated = true;
        spriteRenderer.sprite = pics[0];
    }
}
