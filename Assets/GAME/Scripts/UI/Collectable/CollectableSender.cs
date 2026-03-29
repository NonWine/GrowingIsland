using System;
using System.Collections;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class CollectableSender : MonoBehaviour
{
    private CollectableAnimationData.SendingData animData;
    [FormerlySerializedAs("_collectableVisualPartPrefab")]
    [SerializeField] private CollectableVisualPart collectableVisualPartPrefab;
    [FormerlySerializedAs("_amount")]
    [SerializeField] private int amount;
    [FormerlySerializedAs("_size")]
    [SerializeField] private Vector2 size;

    [FormerlySerializedAs("_collectableVisualParts")]
    [SerializeField, ReadOnly] private CollectableVisualPart[] collectableVisualParts;

    private Camera cam => Camera.main;
    private CollectableWallet wallet;
    private Coroutine coroutine;
    private Action onEndCallback;
    private int count;

    #region Editor
    private void OnValidate() 
        => SetRefs();

    [Button]
    private void AddVisuals()
    {
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        var sprite = transform.parent.FindDeepChild<Image>("Icon").sprite;

        for (int i = 0; i < amount; i++)
        {
            var item = Instantiate(collectableVisualPartPrefab, transform);
            item.GetComponent<RectTransform>().sizeDelta = size;
            item.GetComponent<Image>().sprite = sprite;
            item.name = $"Visual ({i})";
            item.gameObject.SetActive(false);
        }

        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
    
        collectableVisualParts = GetComponentsInChildren<CollectableVisualPart>(true);
    }
    #endregion

    #region Init
    private void OnEnable()
    {
        foreach (var visual in collectableVisualParts)
            visual.onEndSending += OnEndSending;
    }

    private void OnDisable()
    {
        foreach (var visual in collectableVisualParts)
            visual.onEndSending -= OnEndSending;
    }
    
    public void Initialize(CollectableWallet wallet) 
        => this.wallet = wallet;
    #endregion

    #region Callbacks
    private void OnEndSending()
    {
        if (onEndCallback == null)
            return;

        count--;
        if (count <= 0)
        {
            onEndCallback?.Invoke();
            onEndCallback = null;
        }
    }
    #endregion
    
    public void Send(RectTransform start, int amount, RectTransform target, Action onEnd = null)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        count = Mathf.Min(amount, collectableVisualParts.Length);
        onEndCallback = onEnd;

        coroutine = StartCoroutine(SendC(start, target, amount));
    }

    public void SendOne(Transform startPos, RectTransform target, Action onEnd = null)
    {
        var collectable = collectableVisualParts.First( x=> x.gameObject.activeSelf == false);
        collectable.transform.SetParent(target);
        var pos = cam.WorldToScreenPoint(startPos.position);
        collectable.Initialize(pos);
        collectable.MoveTo(target, wallet, 1);
    }
    
    public void SendOne(Transform startPos, RectTransform target, int countAdd, Action onEnd = null)
    {
        var collectable = collectableVisualParts.First( x=> x.gameObject.activeSelf == false);
        collectable.transform.SetParent(target);
        var pos = cam.WorldToScreenPoint(startPos.position);
        collectable.Initialize(pos);
        collectable.MoveTo(target, wallet, countAdd);
    }

    private IEnumerator SendC(RectTransform start, RectTransform target, int amount)
    {
        int addingValue;
        int lastAddingValue;
        if(amount <= collectableVisualParts.Length)
        {
            addingValue = 1;
            lastAddingValue = addingValue;
        }
        else
        {
            addingValue = amount / collectableVisualParts.Length;
            lastAddingValue = addingValue + amount % collectableVisualParts.Length;
        }
        
        for (int i = 0; i < count; i++)
        {
            var collectable = collectableVisualParts[i];

            collectable.transform.SetParent(target);
            collectable.Initialize(start);
            collectable.MoveTo(target, wallet, (i != count - 1) ? addingValue : lastAddingValue);

            var delay = animData.DelayBetween * animData.DelayCurve.Evaluate(((float)i / count));

            if (delay != 0)
                yield return new WaitForSeconds(delay);
        }
    }
    
}
