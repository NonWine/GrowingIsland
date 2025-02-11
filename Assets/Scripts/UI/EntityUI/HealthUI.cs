using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private RectTransform _canvas;
    

    public void SetHealth(float value)
    {
        _slider.maxValue = value;
        _slider.value = value;
    }

    public void GetDamageUI(float count)
    {
        DOVirtual.Float(_slider.value, _slider.value - count, 0.25f, x =>
        {
            _slider.value = x;
        }).SetEase(Ease.Linear);
    }

    public void Tick()
    {
        SetRightRotation();
    }

    private void SetRightRotation()
    {
        _canvas.rotation = Quaternion.Euler(40f, transform.rotation.y, 0f);
    }
    
}

