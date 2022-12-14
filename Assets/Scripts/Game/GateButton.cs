using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    public delegate void ButtonPress();
    public event ButtonPress OnButtonPress;
    public event ButtonPress OnButtonUnpress;

    [SerializeField]
    private bool hold;
    private bool active;
    private int touching;
    
    [SerializeField]
    private Sprite[] pics;

    private SpriteRenderer spriteRenderer;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        source = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!active)
            return;
        
        if (!collider.CompareTag("Player") && !collider.CompareTag("Friend"))
            return;
        OnButtonPress?.Invoke();

        // Play press sound
        if(!source.isPlaying)
            source.Play();
        touching++;
        spriteRenderer.sprite = pics[1];

        if(!hold)
            active = false;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!hold || !active)
            return;

        if (collider.CompareTag("Player") || collider.CompareTag("Friend"))
        {
            touching--;
            Debug.Log("Touching is at " + touching);

            if (touching <= 0)
            {
                OnButtonUnpress?.Invoke();
                spriteRenderer.sprite = pics[0];
            }
                
        }
    }
}
