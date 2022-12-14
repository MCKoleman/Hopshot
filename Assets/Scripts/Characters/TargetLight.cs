using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color choice)
    {
        spriteRenderer.color = choice;
    }
}
