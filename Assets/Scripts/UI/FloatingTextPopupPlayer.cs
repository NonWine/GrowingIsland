using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingTextPopupPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _popupPrefab;
    [SerializeField] private FloatingTextPopupSettings _settings = new();

    public void Play(Transform parent)
    {
        if (parent == null)
            return;

        Play(parent, _settings.DefaultText);
    }

    public void Play(Transform parent, string text)
    {
        if (_popupPrefab == null || parent == null)
            return;

        TMP_Text popup = Instantiate(_popupPrefab, parent);
        RectTransform popupRect = popup.rectTransform;
        Vector2 startPosition = _settings.Offset;

        popup.text = string.IsNullOrWhiteSpace(text) ? _settings.DefaultText : text;
        popup.color = _settings.Color;
        popupRect.anchoredPosition = startPosition;
        popupRect.localScale = Vector3.one * _settings.StartScale;

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Join(DOTween.To(
                () => popupRect.anchoredPosition,
                value => popupRect.anchoredPosition = value,
                startPosition + Vector2.up * _settings.Rise,
                _settings.Duration)
            .SetEase(Ease.OutQuad));
        popupSequence.Join(
            popupRect.DOScale(_settings.EndScale, _settings.Duration).SetEase(Ease.OutBack));
        popupSequence.Join(DOTween.To(
                () => popup.color,
                value => popup.color = value,
                new Color(_settings.Color.r, _settings.Color.g, _settings.Color.b, 0f),
                _settings.Duration)
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
