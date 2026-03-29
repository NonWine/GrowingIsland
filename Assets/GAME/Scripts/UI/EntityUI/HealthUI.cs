using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class HealthUI : MonoBehaviour
{
    [FormerlySerializedAs("_slider")]
    [SerializeField] private Slider slider;
    [FormerlySerializedAs("_canvas")]
    [SerializeField] private RectTransform canvas;
    

    public void SetHealth(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void GetDamageUI(float count)
    {
        DOVirtual.Float(slider.value, slider.value - count, 0.25f, x =>
        {
            slider.value = x;
        }).SetEase(Ease.Linear);
    }

    public void Tick()
    {
        SetRightRotation();
    }

    private void SetRightRotation()
    {
        canvas.rotation = Quaternion.Euler(40f, transform.rotation.y, 0f);
    }
    
}

