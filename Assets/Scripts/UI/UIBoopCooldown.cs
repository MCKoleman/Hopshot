using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBoopCooldown : MonoBehaviour
{
    [SerializeField]
    private Image sliderFill;
    [SerializeField]
    private Image cooldownIcon;
    [SerializeField]
    private float sliderLerpSpeed = 6.0f;
    [SerializeField]
    private float cooldownLerpSpeed = 6.0f;
    [SerializeField]
    private float cooldownBounceSpeed = 0.2f;
    [SerializeField]
    private Color cooldownDisabledColor;
    [SerializeField]
    private Color cooldownEnabledColor;
    [SerializeField]
    private Color sliderDisabledColor;
    [SerializeField]
    private Color sliderEnabledColor;

    private Color cooldownTargetColor;
    private Color sliderTargetColor;
    private float targetSliderFill;

    private void Start()
    {
        targetSliderFill = 1.0f;
        cooldownTargetColor = cooldownEnabledColor;
        sliderTargetColor = sliderEnabledColor;
        cooldownIcon.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        // Lerp to target color, bouncing on complete
        if(!MathUtils.IsAlmostColor(cooldownIcon.color, cooldownTargetColor, 2))
        {
            cooldownIcon.color = Color.Lerp(cooldownIcon.color, cooldownTargetColor, cooldownLerpSpeed * Time.deltaTime);
            if(MathUtils.IsAlmostColor(cooldownIcon.color, cooldownTargetColor, 2))
            {
                // Bounce on reaching enabled color
                cooldownIcon.color = cooldownTargetColor;
                if (MathUtils.IsAlmostColor(cooldownEnabledColor, cooldownTargetColor, 2))
                    cooldownIcon.transform.DOScale(1.2f, cooldownBounceSpeed)
                        .OnComplete(() => cooldownIcon.transform.DOScale(1.0f, cooldownBounceSpeed));
            }
        }
        
        // Lerp slider to target color
        if (!MathUtils.IsAlmostColor(sliderFill.color, sliderTargetColor, 2))
        {
            sliderFill.color = Color.Lerp(sliderFill.color, sliderTargetColor, cooldownLerpSpeed * Time.deltaTime);
            if (MathUtils.IsAlmostColor(sliderFill.color, sliderTargetColor, 2))
            {
                // Bounce on reaching enabled color
                sliderFill.color = sliderTargetColor;
            }
        }

        // Lerp slider to target fill
        if (!MathUtils.AlmostZero(sliderFill.fillAmount - targetSliderFill, 2))
        {
            sliderFill.fillAmount = Mathf.Lerp(sliderFill.fillAmount, targetSliderFill, sliderLerpSpeed * Time.deltaTime);
            if (MathUtils.AlmostZero(sliderFill.fillAmount - targetSliderFill, 2))
            {
                sliderFill.fillAmount = targetSliderFill;
            }
        }
    }

    // Sets the slider fill to the given percent
    public void SetSliderFill(float percent)
    {
        targetSliderFill = 1.0f - percent;
        cooldownTargetColor = (Mathf.Approximately(percent, 0.0f)) ? cooldownEnabledColor : cooldownDisabledColor;
        sliderTargetColor = (Mathf.Approximately(percent, 0.0f)) ? sliderEnabledColor : sliderDisabledColor;
    }
}
