using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectableWallet : MonoBehaviour
{
    [SerializeField, ReadOnly] private TextMeshProUGUI _amountDisplay;
    [SerializeField, ReadOnly] private RectTransform _amountRect;
    [SerializeField, ReadOnly] private RectTransform _targetTransform;
    [SerializeField, ReadOnly] private CollectableSender _collectableSender;
    [SerializeField, ReadOnly] private GameObject _visual;
    [Inject] private StorageManager _storageManager;
    [SerializeField] private Image _image;
    private WalletObj _walletObj;
    public event Action<int> onValueChanged;
    
    
    
    private Tween _punchingAmount;
    private Tween _punchingIcon;
    private Sequence _shakeAmount;
    private Vector3 _startAmountPosition;

   [Inject] private CollectableAnimationData _walletData;

    private int _amount;

    public eCollectable WalletType => _walletObj.TypeWallet;
    
    #region Amount
    //[ShowInInspector, PropertyOrder(-1)]
    public int Amount
    {
        get
        {
            return _storageManager.GetCollectable(_walletObj.TypeWallet);
        }

        private set
        {
           
            onValueChanged?.Invoke(value);
            _storageManager.SetCollectable(_walletObj.TypeWallet, value);
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
        _amountDisplay = GetComponentInChildren<TextMeshProUGUI>(true);
        _amountRect = _amountDisplay.GetComponent<RectTransform>();
        _targetTransform = transform.FindDeepChild<RectTransform>("Icon");
        _collectableSender = GetComponentInChildren<CollectableSender>();
        _visual = transform.FindDeepChild<GameObject>("Visual");
    }
    #endregion

    #region Init
    public void Init(WalletObj walletObj)
    {
        //get fromSave
        _walletObj = walletObj;
        _image.sprite = walletObj.ItemIcon;
        Debug.Log(_storageManager.ToString());
        UpdateAmount(Amount);
        _collectableSender.Initialize(this);

        _startAmountPosition = _amountRect.anchoredPosition;
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
        _amountDisplay.text = NumberFormatter.GetFormatedNumber(amount);
    }

    private void Hide()
    {
        _visual.SetActive(false);
    }

    private void Show()
    {
        if (_visual.activeInHierarchy)
            return;

        _visual.SetActive(true);
    }
    #endregion

    public void Add(int amount, bool isNeededAnimation = true)
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        PlayAmountAnimation();

        if (isNeededAnimation)
        {
            _punchingIcon?.Kill();
            _targetTransform.localScale = Vector3.one;
            _punchingIcon = _targetTransform.DOPunchScale(_walletData.WalletAnimationData.IconAnimation.Scale,
                _walletData.WalletAnimationData.IconAnimation.Time,
                _walletData.WalletAnimationData.IconAnimation.Vibration);
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
        _collectableSender.SendOne(startPos, _targetTransform);
    }
    public void SendOne(Transform startPos, int countAdD)
    {
        _collectableSender.SendOne(startPos, _targetTransform, countAdD);
    }
    
    public void AddWithAnimation(int amount, RectTransform start, Action onEnd = null)
    {
        if(_collectableSender != null)
            _collectableSender.Send(start, amount, _targetTransform, onEnd);
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
            if (_shakeAmount.IsActive() == false || _shakeAmount.IsPlaying() == false)
            {
                _startAmountPosition = _amountRect.anchoredPosition;
                
                _shakeAmount?.Kill();
                _shakeAmount = DOTween.Sequence();
                _shakeAmount.Append(_amountRect.DOShakePosition(1f, 5f, 5, 50, false, true))
                    .SetRelative(false)
                    .OnComplete(() => _amountRect.anchoredPosition = _startAmountPosition);
            }
        }

        ResetHorizontalLayout();
        return remainingAmount >= 0;
        
    }
    
    
    public void RemoveAll() => TryRemove(Amount);

    public bool CanRemove(int amount)
        => Amount - amount >= 0 ? true : false;
    
    
    private void ResetHorizontalLayout() 
        => LayoutRebuilder.ForceRebuildLayoutImmediate(_amountRect);

    #region Specific
    private void PlayAmountAnimation()
    {
        _punchingAmount?.Kill();
        _amountRect.localScale = Vector3.one;
        _punchingAmount = _amountRect.DOPunchScale(_walletData.WalletAnimationData.AmountAnimation.Scale,
            _walletData.WalletAnimationData.AmountAnimation.Time,
            _walletData.WalletAnimationData.AmountAnimation.Vibration);
    }
    #endregion
}
