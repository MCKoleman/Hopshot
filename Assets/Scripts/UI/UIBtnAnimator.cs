using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIBtnAnimator : MonoBehaviour
{
    [SerializeField]
    private float highlightScale = 1.2f;
    [SerializeField]
    private float scaleSpeed = 0.2f;

    public void Highlight()
    {
        this.transform.DOScale(highlightScale, scaleSpeed);
    }

    public void Byelight()
    {
        this.transform.DOScale(1.0f, scaleSpeed);
    }
}
