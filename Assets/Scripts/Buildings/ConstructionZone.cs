using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class ConstructionZone : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [SerializeField] private ConstructionUI constructionUI; // UI прогресу
    [SerializeField] private List<ResourceRequirement> requiredResources; // Потрібні ресурси
    [SerializeField] private Transform _throwPointTarget;
    [SerializeField] private Transform _canvasZone;
    [SerializeField] private ResourceThrowSettings _throwSettings = new();
    [Inject] private CollectableManager _collectableManager;
    [Inject] private ResourcePartObjFactory _resourceFactory;
    [Inject] private Player _playerContainer;
    [Inject] private ResourceData _resourceData;
    private Dictionary<eCollectable, int> currentResources = new Dictionary<eCollectable, int>();
    private Coroutine resourceDeliveryCoroutine;
    private bool isPlayerInZone = false;
    private bool isConstructionComplete = false;

    public event Action OnComplete;

    private void Start()
    {
        InitializeResourceCounters();

        // Оновлюємо UI
     //   constructionUI.InitializeUI(requiredResources);
    }

    private void OnValidate()
    {
        if (constructionUI != null)
            constructionUI.InitializeUI(requiredResources);
        InitializeResourceCounters();
    }

    private void InitializeResourceCounters()
    {
        currentResources.Clear();
        foreach (var resource in requiredResources)
        {
            currentResources.TryAdd(resource.WalletObj.TypeWallet, 0);
        }
    }

    private IEnumerator TransferResources()
    {
        float delay = _resourceData.DelayPerResource;

        foreach (var requirement in requiredResources)
        {
            yield return TransferRequirement(requirement, delay);
        }
        yield return new WaitForSeconds(_resourceData.DelayPerResource);
        CheckCompletion();
    }

    private async void CheckCompletion()
    {
        if (isConstructionComplete) return;

        bool isComplete = requiredResources.All(r => currentResources[r.WalletObj.TypeWallet] >= r.amount);

        if (isComplete)
        {
            isConstructionComplete = true;
            StopResourceDelivery();
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void StartResourceDelivery()
    {
        if (resourceDeliveryCoroutine == null)
        {
            _canvasZone.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutBack);
            resourceDeliveryCoroutine = StartCoroutine(TransferResources());
        }
    }

    private void StopResourceDelivery()
    {
        if (resourceDeliveryCoroutine != null)
        {
            _canvasZone.transform.DOScale(1f, 0.15f).SetEase(Ease.Linear);

            StopCoroutine(resourceDeliveryCoroutine);
            resourceDeliveryCoroutine = null;
        }
    }
    

    public void PlayerEnter()
    {
        isPlayerInZone = true;
        StartResourceDelivery();
    }

    public void PlayerExit()
    {
        isPlayerInZone = false;
        StopResourceDelivery();
    }

    private IEnumerator TransferRequirement(ResourceRequirement requirement, float delay)
    {
        var wallet = _collectableManager.GetWallet(requirement.WalletObj.TypeWallet);
        if (wallet == null)
            yield break;

        int totalNeeded = requirement.amount - currentResources[requirement.WalletObj.TypeWallet];
        int totalAvailable = wallet.Amount;

        if (totalNeeded <= 0 || totalAvailable <= 0)
            yield break;

        float resourcesPerDrop = totalNeeded > totalAvailable
            ? (float)totalAvailable / _resourceData.CountResourceInAnimation
            : (float)totalNeeded / _resourceData.CountResourceInAnimation;

        for (int i = 0; i < _resourceData.CountResourceInAnimation; i++)
        {
            if (totalNeeded <= 0 || totalAvailable <= 0)
                break;

            int toRemove = Mathf.Min(Mathf.CeilToInt(resourcesPerDrop), totalNeeded, totalAvailable);

            if (!wallet.TryRemove(toRemove))
            {
                Debug.Log("Недостатньо ресурсів у гравця!");
                break;
            }

            totalNeeded -= toRemove;
            totalAvailable -= toRemove;
            currentResources[requirement.WalletObj.TypeWallet] += toRemove;

            AnimateResourceDrop(requirement, toRemove);
            UpdateUi(requirement);

            if (AllRequirementsMet())
                constructionUI.Hide();

            yield return new WaitForSeconds(delay);
        }
    }

    private void AnimateResourceDrop(ResourceRequirement requirement, int count)
    {
        var res = _resourceFactory.Create(type: requirement.WalletObj.TypeWallet);
        res.transform.SetParent(_throwPointTarget, true);
        res.transform.position = _playerContainer.ResourceStartPoint.transform.position;
        res.transform.rotation = _resourceData.StartRotation;

        var targetOffset2D = UnityEngine.Random.insideUnitCircle * _throwSettings.LandingSpread;
        var targetLocal = new Vector3(targetOffset2D.x, 0f, targetOffset2D.y);

        var seq = DOTween.Sequence();
        seq.Append(res.transform.DOLocalJump(targetLocal, _resourceData.JumpPower, _resourceData.NumJumps, _resourceData.Duration)
            .SetEase(Ease.OutQuad));
        seq.Join(res.transform.DOLocalRotate(new Vector3(0f, 360f * _throwSettings.SpinRevolutions, 0f),
            _resourceData.Duration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
        seq.Append(res.transform.DOScale(_throwSettings.ArrivalPopScale, _throwSettings.ArrivalPopDuration).SetEase(Ease.OutBack));
        seq.Append(res.transform.DOScale(1f, _throwSettings.ArrivalPopReturnDuration).SetEase(Ease.InOutSine));
        seq.OnComplete(() =>
        {
            res.PickUp(transform, CollectStrategyType.Player,0);
        });
    }

    private void UpdateUi(ResourceRequirement requirement)
    {
        constructionUI.UpdateUI(requirement.WalletObj.TypeWallet,
            currentResources[requirement.WalletObj.TypeWallet],
            requirement.amount);
    }

    private bool AllRequirementsMet() => requiredResources.All(IsRequirementComplete);

    private bool IsRequirementComplete(ResourceRequirement requirement) =>
        currentResources.TryGetValue(requirement.WalletObj.TypeWallet, out var current) && current >= requirement.amount;
}

[System.Serializable]
public class ResourceRequirement
{
    public WalletObj WalletObj;
    public int amount;        // Необхідна кількість
}

[System.Serializable]
public class ResourceThrowSettings
{
    public float LandingSpread = 0.2f;
    public float SpinRevolutions = 1.25f;
    public float ArrivalPopScale = 1.2f;
    public float ArrivalPopDuration = 0.12f;
    public float ArrivalPopReturnDuration = 0.08f;
}
