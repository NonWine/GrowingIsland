using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class TextIconView : MonoBehaviour
{
    [FormerlySerializedAs("_icon")]
    [SerializeField] private Image icon;
    [FormerlySerializedAs("_text")]
    [SerializeField] private TMP_Text text;

    public void SetIconView(Sprite sprite, string label)
    {
        icon.sprite = sprite;
        text.SetText(label);
    }

    public void UpdateText(string label) => text.text = label;
}
