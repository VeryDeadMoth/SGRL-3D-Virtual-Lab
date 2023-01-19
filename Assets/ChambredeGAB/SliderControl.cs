using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    float progress = 1;
    public Image rempl;

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
      
        progress -= 0.2f;
        //slider.value = progress;
        
        LeanTween.value(gameObject,UpdateValue, oldProgress, progress, 0.5f).setEaseOutCubic();
        // on reset progress si la bare fini a zero 
        if(progress <= 0.1)
        {
            progress = 1;
        }
    }

    public void UpdateValue(float newValue)
    {
        
        rempl.fillAmount = newValue;
    }
}
