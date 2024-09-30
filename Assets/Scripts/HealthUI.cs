using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private RectTransform _canvas;
    
    private void Awake()
    {
        _slider.onValueChanged.AddListener(ChangeHpText);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(ChangeHpText);
    }

    public void SetHealth(float value)
    {
        _slider.maxValue = value;
        _slider.value = value;
    }

    public void GetDamageUI(int count)
    {
        _slider.value = count;
    }

    public void Tick()
    {
        SetRightRotation();
    }

    private void SetRightRotation()
    {
        _canvas.rotation = Quaternion.Euler(40f, transform.rotation.y, 0f);
    }

    private void ChangeHpText(float value)
    {
        _hpText.text  = _slider.value.ToString() + "/" + _slider.maxValue.ToString();
        if (_slider.value <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

