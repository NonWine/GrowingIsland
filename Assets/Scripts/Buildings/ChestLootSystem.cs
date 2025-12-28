using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Inject] private ResourcePartObjFactory _resourcesFactory;

    private Coroutine _lootCoroutine;
    private bool _isLooting;
    private bool canBeDestroy;
    private float _currentTimer;

    [System.Serializable]
    public struct LootData
    {
        public eCollectable Type;
        public int Amount;
    }

    public void PlayerEnter()
    {
        if (_isLooting) return;
        StopAllCoroutines();
        Debug.Log("start Coroutine");
        _lootCoroutine = StartCoroutine(LootProcess());
    }

    public void PlayerExit()
    {
        if(canBeDestroy) return;
        
        if (_lootCoroutine != null)
        {
            StopCoroutine(_lootCoroutine);
            _lootCoroutine = null;
        }
        
        _isLooting = false;
        _currentTimer = 0;
        
        // Reset visual feedback if needed
        _chestVisual.DOKill();
        _chestVisual.localScale = Vector3.one;
    }

    private IEnumerator LootProcess()
    {
        _isLooting = true;
        _currentTimer = 0;

        // Shake animation while looting
        _chestVisual.DOShakePosition(_lootTime, 0.1f, 10, 90, false, false).SetEase(Ease.Linear);
        _chestVisual.DOScale(1.1f, _lootTime).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(_lootTime);
        canBeDestroy = true;
     
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

                resource.ResetPool();
                // Jump out animation
                Vector3 jumpTarget = startPos + Random.insideUnitSphere * 2f;
                jumpTarget.y = startPos.y; // Keep it on the same plane or adjust as needed

                resource.transform.DOJump(jumpTarget, 2f, 1, 0.5f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(resource.SetIdle);
            }
        }
    }

    private void DestroyChest()
    {
        _chestVisual.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
