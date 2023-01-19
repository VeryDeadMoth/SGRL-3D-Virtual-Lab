using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    float progress = 0;
    public Slider slider;

    public void OnEnable()
    {
        GameManagerPopupTest1.OnPopupOpensEvent += ChangeValue;
    }

    public void OnSliderChanged(float value)
    {
        print("Value : " + value);
    }

    public void ChangeValue()
    {
        float oldProgress = progress;
        progress += 0.05f;
        //slider.value = progress;
        LeanTween.value(gameObject,UpdateValue, oldProgress, progress, 0.5f).setEaseOutCubic();
    }

    public void UpdateValue(float newValue)
    {
        slider.value = newValue;
    }
}
