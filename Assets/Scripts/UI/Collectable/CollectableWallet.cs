using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.Serialization;

public class CollectableWallet : MonoBehaviour
{
    [FormerlySerializedAs("_amountDisplay")]
    [SerializeField, ReadOnly] private TextMeshProUGUI amountDisplay;
    [FormerlySerializedAs("_amountRect")]
    [SerializeField, ReadOnly] private RectTransform amountRect;
    [FormerlySerializedAs("_targetTransform")]
    [SerializeField, ReadOnly] private RectTransform targetTransform;
    [FormerlySerializedAs("_collectableSender")]
    [SerializeField, ReadOnly] private CollectableSender collectableSender;
    [FormerlySerializedAs("_visual")]
    [SerializeField, ReadOnly] private GameObject visual;
    [Inject] private StorageManager storageManager;
    [FormerlySerializedAs("_image")]
    [SerializeField] private Image image;
    private WalletObj walletObj;
    public event Action<int> onValueChanged;
    
    
    
    private Tween punchingAmount;
    private Tween punchingIcon;
    private Sequence shakeAmount;
    private Vector3 startAmountPosition;

   [Inject] private CollectableAnimationData walletData;

    private int amount;

    public eCollectable WalletType => walletObj.TypeWallet;
    
    #region Amount
    //[ShowInInspector, PropertyOrder(-1)]
    public int Amount
    {
        get
        {
            return storageManager.GetCollectable(walletObj.TypeWallet);
        }

        private set
        {
           
            onValueChanged?.Invoke(value);
            storageManager.SetCollectable(walletObj.TypeWallet, value);
        }
    }
    #endregion
    
    
    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }
    
    [Button]
    private void SetRefs()
    {
        amountDisplay = GetComponentInChildren<TextMeshProUGUI>(true);
        amountRect = amountDisplay.GetComponent<RectTransform>();
        targetTransform = transform.FindDeepChild<RectTransform>("Icon");
        collectableSender = GetComponentInChildren<CollectableSender>();
        visual = transform.FindDeepChild<GameObject>("Visual");
    }
    #endregion

    #region Init
    public void Init(WalletObj walletObj)
    {
        //get fromSave
        this.walletObj = walletObj;
        image.sprite = walletObj.ItemIcon;
        Debug.Log(storageManager.ToString());
        UpdateAmount(Amount);
        collectableSender.Initialize(this);

        startAmountPosition = amountRect.anchoredPosition;
    }

    private void OnEnable()
    {
        onValueChanged += UpdateAmount;
    }

    private void OnDisable()
    {
        onValueChanged -= UpdateAmount;
    }
    #endregion

    #region Callbacks
    private void UpdateAmount(int amount)
    {
        amountDisplay.text = NumberFormatter.GetFormatedNumber(amount);
    }

    private void Hide()
    {
        visual.SetActive(false);
    }

    private void Show()
    {
        if (visual.activeInHierarchy)
            return;

        visual.SetActive(true);
    }
    #endregion

    public void Add(int amount, bool isNeededAnimation = true)
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        PlayAmountAnimation();

        if (isNeededAnimation)
        {
            punchingIcon?.Kill();
            targetTransform.localScale = Vector3.one;
            punchingIcon = targetTransform.DOPunchScale(walletData.WalletAnimationData.IconAnimation.Scale,
                walletData.WalletAnimationData.IconAnimation.Time,
                walletData.WalletAnimationData.IconAnimation.Vibration);
        }

        Amount += amount;
        ResetHorizontalLayout();
    }

    public async void AddDelay(int amount, int miliSec, bool amin = true)
    {
        await UniTask.Delay(miliSec);
        Add(amount,amin);
    }

    public void SendOne(Transform startPos)
    {
        collectableSender.SendOne(startPos, targetTransform);
    }
    public void SendOne(Transform startPos, int countAdD)
    {
        collectableSender.SendOne(startPos, targetTransform, countAdD);
    }
    
    public void AddWithAnimation(int amount, RectTransform start, Action onEnd = null)
    {
        if(collectableSender != null)
            collectableSender.Send(start, amount, targetTransform, onEnd);
        else
            Debug.LogError("Collectable sender wasn't setup!");
    }

    public bool TryRemove(int amount)
    {
        int remainingAmount = Amount - amount;
        if (remainingAmount >= 0)
        {
            PlayAmountAnimation();
            Amount = remainingAmount;
        }
        else
        {
            if (shakeAmount.IsActive() == false || shakeAmount.IsPlaying() == false)
            {
                startAmountPosition = amountRect.anchoredPosition;
                
                shakeAmount?.Kill();
                shakeAmount = DOTween.Sequence();
                shakeAmount.Append(amountRect.DOShakePosition(1f, 5f, 5, 50, false, true))
                    .SetRelative(false)
                    .OnComplete(() => amountRect.anchoredPosition = startAmountPosition);
            }
        }

        ResetHorizontalLayout();
        return remainingAmount >= 0;
        
    }
    
    
    public void RemoveAll() => TryRemove(Amount);

    public bool CanRemove(int amount)
        => Amount - amount >= 0 ? true : false;
    
    
    private void ResetHorizontalLayout() 
        => LayoutRebuilder.ForceRebuildLayoutImmediate(amountRect);

    #region Specific
    private void PlayAmountAnimation()
    {
        punchingAmount?.Kill();
        amountRect.localScale = Vector3.one;
        punchingAmount = amountRect.DOPunchScale(walletData.WalletAnimationData.AmountAnimation.Scale,
            walletData.WalletAnimationData.AmountAnimation.Time,
            walletData.WalletAnimationData.AmountAnimation.Vibration);
    }
    #endregion
}

