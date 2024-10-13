using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableVisualPart : MonoBehaviour
{
    private CollectableAnimationData.SendingData _animData;
    [SerializeField, ReadOnly] private RectTransform _rectTransform;
    [SerializeField, ReadOnly] private Transform _parent;

    private Sequence _animation;

    public event Action onEndSending;

    #region Editor
    private void OnValidate() 
        => SetRefs();

    [Button]
    private void SetRefs()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parent = transform.parent;
    }
    #endregion

    public void Initialize(RectTransform startTransform)
    {
        transform.position = startTransform.position;
        Initialize();
    }
    
    public void Initialize(Transform startTransform)
    {
        transform.position = startTransform.position;
        Initialize();
    }

    public void Initialize(Vector3 startTransform)
    {
        transform.position = startTransform;
        Initialize();
    }
    
    private void Initialize()
    {
        _animation?.Kill();
        _rectTransform.DOKill();
        
        _rectTransform.localRotation = Quaternion.identity;
        _rectTransform.localScale = Vector3.one * 1f;
    }

    public void MoveTo(RectTransform target, CollectableWallet wallet, int value)
    {
        gameObject.SetActive(true);
        _animation?.Kill();

        _animation = DOTween.Sequence();

        _animation
            // .AppendCallback(() =>
            // {
            //     _rectTransform.DOScale(Vector2.one * _animData.FirstStage.ScaleFactor, _animData.FirstStage.Time)
            //         .SetEase(_animData.FirstStage.ScaleEase);
            // })
            // // .Append(_rectTransform.DOAnchorPos(GetOffset(), _animData.FirstStage.Time)
            // //     .SetEase(_animData.FirstStage.MoveEase)
            // //     .SetRelative(true))
            // .Append(_rectTransform.DOScale(Vector2.one * _animData.SecondStage.ScaleFactor, _animData.SecondStage.Time))
            //     .SetEase(_animData.SecondStage.ScaleEase)
            .AppendCallback(() =>
            {
                _rectTransform.DOScale(Vector2.one * _animData.ThirdStage.ScaleFactor, _animData.ThirdStage.Time)
                    .SetEase(_animData.ThirdStage.Ease);
            })
            .AppendInterval(Random.Range(_animData.RandomRange.Min, _animData.RandomRange.Max))
            .Append(_rectTransform.DOMove(target.position, _animData.ThirdStage.Time).OnStart(() =>
                {
                    _rectTransform.DOScale(Vector3.zero,  _animData.ThirdStage.Time * 4f);
                })
                .SetEase(_animData.ThirdStage.Ease))
            .AppendCallback(() => wallet.Add(value, false))
            .AppendCallback(() => { target.DOScale(1.2f, 0.2f).OnComplete(() =>
                {
                    target.DOScale(1f, 0.2f);
                });
            });

        _animation.OnComplete(() =>
        {
            onEndSending?.Invoke();
            transform.SetParent(_parent);
            gameObject.SetActive(false);
        });
    }

    private Vector2 GetOffset()
    {
        float angle = Random.Range(0, 360f);
        float radius = Random.Range(_animData.Radius * (1f - _animData.RadiusThickness), _animData.Radius);

        return (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * radius) + _animData.FirstStage.Offset;
    }
}
