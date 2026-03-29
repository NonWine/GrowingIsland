using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

public class CollectableVisualPart : MonoBehaviour
{
    private CollectableAnimationData.SendingData animData;
    [FormerlySerializedAs("_rectTransform")]
    [SerializeField, ReadOnly] private RectTransform rectTransform;
    [FormerlySerializedAs("_parent")]
    [SerializeField, ReadOnly] private Transform parent;

    private Sequence animation;

    public event Action onEndSending;

    #region Editor
    private void OnValidate() 
        => SetRefs();

    [Button]
    private void SetRefs()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent;
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
        animation?.Kill();
        rectTransform.DOKill();
        
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one * 1f;
    }

    public void MoveTo(RectTransform target, CollectableWallet wallet, int value)
    {
        gameObject.SetActive(true);
        animation?.Kill();

        animation = DOTween.Sequence();

        animation
            // .AppendCallback(() =>
            // {
            //     rectTransform.DOScale(Vector2.one * animData.FirstStage.ScaleFactor, animData.FirstStage.Time)
            //         .SetEase(animData.FirstStage.ScaleEase);
            // })
            // // .Append(rectTransform.DOAnchorPos(GetOffset(), animData.FirstStage.Time)
            // //     .SetEase(animData.FirstStage.MoveEase)
            // //     .SetRelative(true))
            // .Append(rectTransform.DOScale(Vector2.one * animData.SecondStage.ScaleFactor, animData.SecondStage.Time))
            //     .SetEase(animData.SecondStage.ScaleEase)
            .AppendCallback(() =>
            {
                rectTransform.DOScale(Vector2.one * animData.ThirdStage.ScaleFactor, animData.ThirdStage.Time)
                    .SetEase(animData.ThirdStage.Ease);
            })
            .AppendInterval(Random.Range(animData.RandomRange.Min, animData.RandomRange.Max))
            .Append(rectTransform.DOMove(target.position, animData.ThirdStage.Time).OnStart(() =>
                {
                    rectTransform.DOScale(Vector3.zero,  animData.ThirdStage.Time * 4f);
                })
                .SetEase(animData.ThirdStage.Ease))
            .AppendCallback(() => wallet.Add(value, false))
            .AppendCallback(() => { target.DOScale(1.2f, 0.2f).OnComplete(() =>
                {
                    target.DOScale(1f, 0.2f);
                });
            });

        animation.OnComplete(() =>
        {
            onEndSending?.Invoke();
            transform.SetParent(parent);
            gameObject.SetActive(false);
        });
    }

    private Vector2 GetOffset()
    {
        float angle = Random.Range(0, 360f);
        float radius = Random.Range(animData.Radius * (1f - animData.RadiusThickness), animData.Radius);

        return (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * radius) + animData.FirstStage.Offset;
    }
}
