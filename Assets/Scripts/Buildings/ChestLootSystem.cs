using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class ChestLootSystem : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [Header("Settings")]
    [SerializeField] private float _lootTime = 3f;
    [SerializeField] private List<LootData> _lootItems;
    [SerializeField] private Transform _spawnPoint;
    
    [Header("Visuals")]
    [SerializeField] private Transform _chestVisual;
    [SerializeField] private GameObject _timerUI; 
    [SerializeField] private Image _timerFillImage;

    [Inject] private ResourcePartObjFactory _resourcesFactory;

    private Coroutine _lootCoroutine;
    private bool _isLooting;
    private float _currentTimer;
    private bool _canBeDestroy;

    [System.Serializable]
    public struct LootData
    {
        public eCollectable Type;
        public int Amount;
    }

    private void Start()
    {
        if (_timerUI != null) _timerUI.SetActive(false);
        if (_timerFillImage != null) _timerFillImage.fillAmount = 0;
    }

    public void PlayerEnter()
    {
        if (_isLooting || _canBeDestroy) return;
        
        _lootCoroutine = StartCoroutine(LootProcess());
    }

    public void PlayerExit()
    {
        if (_canBeDestroy) return;

        if (_lootCoroutine != null)
        {
            StopCoroutine(_lootCoroutine);
            _lootCoroutine = null;
        }
        
        _isLooting = false;
        _currentTimer = 0;
        
        if (_timerUI != null) _timerUI.SetActive(false);
        if (_timerFillImage != null) _timerFillImage.fillAmount = 0;

        // Reset visual feedback if needed
        
            _chestVisual.DOKill();
            _chestVisual.localScale = Vector3.one;
        
    }

    private IEnumerator LootProcess()
    {
        _isLooting = true;
        _currentTimer = 0;

        if (_timerUI != null) _timerUI.SetActive(true);

        // Shake animation while looting
        _chestVisual.DOShakePosition(_lootTime, 0.1f, 10, 90, false, false).SetEase(Ease.Linear);
        _chestVisual.DOScale(1.1f, _lootTime).SetEase(Ease.InQuad);

        while (_currentTimer < _lootTime)
        {
            _currentTimer += Time.deltaTime;
            
            if (_timerFillImage != null)
            {
                _timerFillImage.fillAmount = _currentTimer / _lootTime;
            }
            
            yield return null;
        }

        if (_timerUI != null) _timerUI.SetActive(false);
        _canBeDestroy = true;
        SpawnLoot();
        DestroyChest();
    }

    private void SpawnLoot()
    {
        foreach (var loot in _lootItems)
        {
            for (int i = 0; i < loot.Amount; i++)
            {
                var resource = _resourcesFactory.Create(loot.Type);
                Vector3 startPos = _spawnPoint != null ? _spawnPoint.position : transform.position;
                resource.transform.position = startPos;
                
                // Jump out animation
                Vector3 jumpTarget = startPos + Random.insideUnitSphere * 3f;
                jumpTarget.y = startPos.y; // Keep it on the same plane or adjust as needed

                resource.transform.DOJump(jumpTarget, 2f, 1, 0.5f)
                    .SetEase(Ease.OutQuad);
                resource.ResetPool();
            }
        }
    }

    private void DestroyChest()
    {
        if (_canBeDestroy) return;
        _canBeDestroy = true;

        _chestVisual.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
