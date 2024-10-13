using System;
using System.Collections;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CollectableSender : MonoBehaviour
{
    private CollectableAnimationData.SendingData _animData;
    [SerializeField] private CollectableVisualPart _collectableVisualPartPrefab;
    [SerializeField] private int _amount;
    [SerializeField] private Vector2 _size;

    [SerializeField, ReadOnly] private CollectableVisualPart[] _collectableVisualParts;

    private Camera _cam => Camera.main;
    private CollectableWallet _wallet;
    private Coroutine _coroutine;
    private Action _onEndCallback;
    private int _count;

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

        for (int i = 0; i < _amount; i++)
        {
            var item = Instantiate(_collectableVisualPartPrefab, transform);
            item.GetComponent<RectTransform>().sizeDelta = _size;
            item.GetComponent<Image>().sprite = sprite;
            item.name = $"Visual ({i})";
            item.gameObject.SetActive(false);
        }

        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
    
        _collectableVisualParts = GetComponentsInChildren<CollectableVisualPart>(true);
    }
    #endregion

    #region Init
    private void OnEnable()
    {
        foreach (var visual in _collectableVisualParts)
            visual.onEndSending += OnEndSending;
    }

    private void OnDisable()
    {
        foreach (var visual in _collectableVisualParts)
            visual.onEndSending -= OnEndSending;
    }
    
    public void Initialize(CollectableWallet wallet) 
        => _wallet = wallet;
    #endregion

    #region Callbacks
    private void OnEndSending()
    {
        if (_onEndCallback == null)
            return;

        _count--;
        if (_count <= 0)
        {
            _onEndCallback?.Invoke();
            _onEndCallback = null;
        }
    }
    #endregion
    
    public void Send(RectTransform start, int amount, RectTransform target, Action onEnd = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _count = Mathf.Min(amount, _collectableVisualParts.Length);
        _onEndCallback = onEnd;

        _coroutine = StartCoroutine(SendC(start, target, amount));
    }

    public void SendOne(Transform startPos, RectTransform target, Action onEnd = null)
    {
        var collectable = _collectableVisualParts.First( x=> x.gameObject.activeSelf == false);
        collectable.transform.SetParent(target);
        var pos = _cam.WorldToScreenPoint(startPos.position);
        collectable.Initialize(pos);
        collectable.MoveTo(target, _wallet, 1);
    }
    
    public void SendOne(Transform startPos, RectTransform target, int countAdd, Action onEnd = null)
    {
        var collectable = _collectableVisualParts.First( x=> x.gameObject.activeSelf == false);
        collectable.transform.SetParent(target);
        var pos = _cam.WorldToScreenPoint(startPos.position);
        collectable.Initialize(pos);
        collectable.MoveTo(target, _wallet, countAdd);
    }

    private IEnumerator SendC(RectTransform start, RectTransform target, int amount)
    {
        int addingValue;
        int lastAddingValue;
        if(amount <= _collectableVisualParts.Length)
        {
            addingValue = 1;
            lastAddingValue = addingValue;
        }
        else
        {
            addingValue = amount / _collectableVisualParts.Length;
            lastAddingValue = addingValue + amount % _collectableVisualParts.Length;
        }
        
        for (int i = 0; i < _count; i++)
        {
            var collectable = _collectableVisualParts[i];

            collectable.transform.SetParent(target);
            collectable.Initialize(start);
            collectable.MoveTo(target, _wallet, (i != _count - 1) ? addingValue : lastAddingValue);

            var delay = _animData.DelayBetween * _animData.DelayCurve.Evaluate(((float)i / _count));

            if (delay != 0)
                yield return new WaitForSeconds(delay);
        }
    }
    
}
