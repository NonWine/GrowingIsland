using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class AutoSizeUILabel : MonoBehaviour
    {
        [FormerlySerializedAs("_padding")]
        [SerializeField] private Padding padding;
        [FormerlySerializedAs("_preferredRect")]
        [SerializeField] private RectTransform preferredRect;
        [FormerlySerializedAs("_useWidht")]
        [FormerlySerializedAs("_useHeight")]
        [SerializeField] private bool useHeight, useWidht;
        private RectTransform rectTransform;

        [ShowInInspector]
        public TextIconView[] Labels => preferredRect.GetComponentsInChildren<TextIconView>(true);
        
        private void OnValidate()
        {
            rectTransform = GetComponent<RectTransform>();
           // UpdateRectToPadding();
        }

        private void Start()
        {
            Canvas.ForceUpdateCanvases();
            UpdateRectToPadding();
        }

        [Button]
        public void UpdateRectToPadding()
        {
            var rect = rectTransform.rect;
            if (useHeight)
            {
                rectTransform.sizeDelta = new Vector2(rect.width + padding.widht, preferredRect.sizeDelta.y + padding.height);
            }

            if (useWidht)
            {
                rectTransform.sizeDelta = new Vector2(preferredRect.sizeDelta.x + padding.widht, rect.height + padding.height);

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
