using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatingTextPopupPlayer : MonoBehaviour
{
    [FormerlySerializedAs("_popupPrefab")]
    [SerializeField] private TMP_Text popupPrefab;
    [FormerlySerializedAs("_settings")]
    [SerializeField] private FloatingTextPopupSettings settings = new();

    public void Play(Transform parent)
    {
        if (parent == null)
            return;

        Play(parent, settings.DefaultText);
    }

    public void Play(Transform parent, string text)
    {
        if (popupPrefab == null || parent == null)
            return;

        TMP_Text popup = Instantiate(popupPrefab, parent);
        RectTransform popupRect = popup.rectTransform;
        Vector2 startPosition = settings.Offset;

        popup.text = string.IsNullOrWhiteSpace(text) ? settings.DefaultText : text;
        popup.color = settings.Color;
        popupRect.anchoredPosition = startPosition;
        popupRect.localScale = Vector3.one * settings.StartScale;

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Join(DOTween.To(
                () => popupRect.anchoredPosition,
                value => popupRect.anchoredPosition = value,
                startPosition + Vector2.up * settings.Rise,
                settings.Duration)
            .SetEase(Ease.OutQuad));
        popupSequence.Join(
            popupRect.DOScale(settings.EndScale, settings.Duration).SetEase(Ease.OutBack));
        popupSequence.Join(DOTween.To(
                () => popup.color,
                value => popup.color = value,
                new Color(settings.Color.r, settings.Color.g, settings.Color.b, 0f),
                settings.Duration)
            .SetEase(Ease.InQuad));
        popupSequence.OnComplete(() => Destroy(popup.gameObject));
    }
}

[Serializable]
public class FloatingTextPopupSettings
{
    public string DefaultText = "+1";
    public Vector2 Offset = new(0f, 28f);
    public float Rise = 24f;
    public float Duration = 0.45f;
    public float StartScale = 0.85f;
    public float EndScale = 1.05f;
    public Color Color = new(1f, 0.94f, 0.74f, 1f);
}
