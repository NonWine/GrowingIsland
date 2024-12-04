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
        // Ініціалізуємо поточний список ресурсів
        foreach (var resource in requiredResources)
        {
            currentResources.TryAdd(resource.WalletObj.TypeWallet, 0);
        }

        // Оновлюємо UI
     //   constructionUI.InitializeUI(requiredResources);
    }

    private void OnValidate()
    {
       constructionUI.InitializeUI(requiredResources);

    }

    private IEnumerator TransferResources()
    {
        float delay = _resourceData.DelayPerResource;

        foreach (var requirement in requiredResources)
        {
        
            
            int totalNeeded = requirement.amount - currentResources[requirement.WalletObj.TypeWallet];
            int totalAvailable = _collectableManager.GetWallet(requirement.WalletObj.TypeWallet).Amount;

            if (totalNeeded <= 0)
                continue; // Якщо ресурси вже достатні, переходимо до наступної вимоги

            // Кількість реальних ресурсів для кожної ітерації
            float resourcesPerDrop = totalNeeded > totalAvailable
                ? (float)totalAvailable / 10f
                : (float)totalNeeded / 10f;

            for (int i = 0; i < _resourceData.CountResourceInAnimation; i++)
            {
                if (totalNeeded <= 0 || totalAvailable <= 0)
                    break; // Якщо ресурси закінчилися, зупиняємо цикл

                int toRemove = Mathf.CeilToInt(resourcesPerDrop);

                // Перевіряємо, чи гравець має достатньо ресурсів
                if (_collectableManager.GetWallet(requirement.WalletObj.TypeWallet).TryRemove(toRemove))
                {
                    // Якщо ресурс успішно забрано, оновлюємо цільовий лічильник
                    totalNeeded -= toRemove;
                    totalAvailable -= toRemove;
                    currentResources[requirement.WalletObj.TypeWallet] += toRemove;
                }
                else
                {
                    Debug.Log("Недостатньо ресурсів у гравця!");
                    break; // Вихід, якщо недостатньо ресурсів
                }

                // Створюємо об'єкт для анімації
                var res = _resourceFactory.Create(type: requirement.WalletObj.TypeWallet);
                res.transform.parent = _throwPointTarget;
                res.transform.position = _playerContainer.ResourceStartPoint.transform.position;
                res.transform.rotation = _resourceData.StartRotation;

                // Анімація ресурсу
                
                res.transform.DOLocalJump(Vector3.zero, _resourceData.JumpPower, _resourceData.NumJumps, _resourceData.Duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        res.DestroyAnim();
                    });

                // Оновлюємо UI
                constructionUI.UpdateUI(requirement.WalletObj.TypeWallet, currentResources[requirement.WalletObj.TypeWallet], requirement.amount);

                if(requiredResources.All(r => currentResources[r.WalletObj.TypeWallet] >= r.amount))
                    constructionUI.Hide();
                
                yield return new WaitForSeconds(delay); // Затримка між ресурсами
            }

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
}

[System.Serializable]
public class ResourceRequirement
{
    public WalletObj WalletObj;
    public int amount;        // Необхідна кількість
}
