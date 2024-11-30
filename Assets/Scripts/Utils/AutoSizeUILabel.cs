using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class AutoSizeUILabel : MonoBehaviour
    {
        [SerializeField] private Padding _padding;
        [SerializeField] private RectTransform _preferredRect;
        [SerializeField] private bool _useHeight, _useWidht;
        private RectTransform _rectTransform;
        
        private void OnValidate()
        {
            _rectTransform = GetComponent<RectTransform>();
            UpdateRectToPadding();
        }

        private void Start()
        {
            Canvas.ForceUpdateCanvases();
            UpdateRectToPadding();
        }

        private void UpdateRectToPadding()
        {
            var rect = _rectTransform.rect;
            if (_useHeight)
            {
                _rectTransform.sizeDelta = new Vector2(rect.width + _padding.widht, _preferredRect.sizeDelta.y + _padding.height);
            }

            if (_useWidht)
            {
                _rectTransform.sizeDelta = new Vector2(_preferredRect.sizeDelta.x + _padding.widht, rect.height + _padding.height);

            }
        }


        [Serializable]
            public struct  Padding
        {
            public float widht;
            public float height;

        }
    }
}
