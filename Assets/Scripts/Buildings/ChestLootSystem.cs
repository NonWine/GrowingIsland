using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using UnityEngine.Serialization;

public class ChestLootSystem : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [Header("Settings")]
    [FormerlySerializedAs("_lootTime")]
    [SerializeField] private float lootTime = 3f;
    [FormerlySerializedAs("_lootItems")]
    [SerializeField] private List<LootData> lootItems;
    [FormerlySerializedAs("_spawnPoint")]
    [SerializeField] private Transform spawnPoint;
    
    [Header("Visuals")]
    [FormerlySerializedAs("_chestVisual")]
    [SerializeField] private Transform chestVisual;
    [FormerlySerializedAs("_timerUI")]
    [SerializeField] private GameObject timerUI;
    [FormerlySerializedAs("_timerFillImage")]
    [SerializeField] private Image timerFillImage;

    [Inject] private ResourcePartObjFactory resourcesFactory;

    private Coroutine lootCoroutine;
    private bool isLooting;
    private float currentTimer;
    private bool canBeDestroy;

    [System.Serializable]
    public struct LootData
    {
        public eCollectable Type;
        public int Amount;
    }

    private void Start()
    {
        if (timerUI != null) timerUI.SetActive(false);
        if (timerFillImage != null) timerFillImage.fillAmount = 0;
    }

    public void PlayerEnter()
    {
        if (isLooting || canBeDestroy) return;
        
        lootCoroutine = StartCoroutine(LootProcess());
    }

    public void PlayerExit()
    {
        if (canBeDestroy) return;

        if (lootCoroutine != null)
        {
            StopCoroutine(lootCoroutine);
            lootCoroutine = null;
        }
        
        isLooting = false;
        currentTimer = 0;
        
        if (timerUI != null) timerUI.SetActive(false);
        if (timerFillImage != null) timerFillImage.fillAmount = 0;

        // Reset visual feedback if needed
        
            chestVisual.DOKill();
            chestVisual.localScale = Vector3.one;
        
    }

    private IEnumerator LootProcess()
    {
        isLooting = true;
        currentTimer = 0;

        if (timerUI != null) timerUI.SetActive(true);

        // Shake animation while looting
        chestVisual.DOShakePosition(lootTime, 0.1f, 10, 90, false, false).SetEase(Ease.Linear);
        chestVisual.DOScale(1.1f, lootTime).SetEase(Ease.InQuad);

        while (currentTimer < lootTime)
        {
            currentTimer += Time.deltaTime;
            
            if (timerFillImage != null)
            {
                timerFillImage.fillAmount = currentTimer / lootTime;
            }
            
            yield return null;
        }

        if (timerUI != null) timerUI.SetActive(false);
        canBeDestroy = true;
        SpawnLoot();
        DestroyChest();
    }

    private void SpawnLoot()
    {
        foreach (var loot in lootItems)
        {
            for (int i = 0; i < loot.Amount; i++)
            {
                var resource = resourcesFactory.Create(loot.Type);
                Vector3 startPos = spawnPoint != null ? spawnPoint.position : transform.position;
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
        if (canBeDestroy) return;
        canBeDestroy = true;

        chestVisual.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
