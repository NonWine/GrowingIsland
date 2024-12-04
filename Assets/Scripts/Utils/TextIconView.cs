using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextIconView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _text;

    public void SetIconView(Sprite sprite, string text)
    {
        _icon.sprite = sprite;
        _text.SetText(text);
    }

    public void UpdateText(string text) => _text.text = text;
}
