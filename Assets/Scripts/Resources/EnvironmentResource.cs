using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

public abstract class EnvironmentResource : MonoBehaviour, IWorldHitDamageable
{
    [FormerlySerializedAs("_resourceWorld")]
    [SerializeField] private ResourceWorld resourceWorld;
    [FormerlySerializedAs("_respawnTime")]
    [SerializeField] private float respawnTime = 10f;
    [Inject] private ResourcePartObjFactory resourcesFactory;
    private float health;

    public eCollectable ResourceType => resourceWorld.TypeWallet;
    
    
    private void Awake()
    {
        isAlive = true;
        health = resourceWorld.Health;
     }
    
    

    protected virtual void AnimTrigDamage(Vector3 sourceWorldPosition, bool isFinalHit)
    {
        transform.DOScale(1.05f, 0.3f / 2)
            .SetEase(Ease.OutBounce) // Use an ease type that fits your animation
            .OnComplete(() => 
            {
                // Scale back down to 1
                transform.DOScale(1f, 0.3f / 2)
                    .SetEase(Ease.OutBounce); // Same easing for consistency
            });    }
    
    

    public virtual void GetDamage(float damage)
    {
        ApplyDamage(damage, GetDefaultHitSource());
    }

    public virtual void GetDamage(float damage, Vector3 sourceWorldPosition)
    {
        ApplyDamage(damage, sourceWorldPosition);
    }

    private void ApplyDamage(float damage, Vector3 sourceWorldPosition)
    {
        if(!isAlive)
            return;
        
        health -= damage;
        bool isFinalHit = health <= 0f;
        if (isFinalHit)
        {
            isAlive = false;
            HandleFinalHit(sourceWorldPosition);
            return;
        }

        AnimTrigDamage(sourceWorldPosition, false);
    }

    protected virtual void HandleFinalHit(Vector3 sourceWorldPosition)
    {
        AnimTrigDamage(sourceWorldPosition, true);
        SpawnResourceDrops();
        HideResourceVisuals();
        BeginRespawn();
    }

    protected virtual Vector3 GetDefaultHitSource()
    {
        return transform.position - transform.forward;
    }

    protected void SpawnResourceDrops()
    {
        for (int i = 0; i < resourceWorld.VisualDrop; i++)
        {
            var resource = resourcesFactory.Create(resourceWorld.TypeWallet);
            resource.transform.position = transform.position;
            var offset = transform.position + Random.insideUnitSphere * 2f;
            offset.y = resource.transform.position.y;
            resource.transform.DOMove(offset, 0.8f).SetEase(Ease.OutQuart);
            resource.ResetPool();
        }
    }

    public bool isAlive { get; set; }

    protected void BeginRespawn()
    {
        StartCoroutine(RespawmProp());
    }

    private IEnumerator RespawmProp()
    {
        yield return new WaitForSeconds(respawnTime);
        isAlive = true;
        health = resourceWorld.Health;
        ShowResourceVisuals();
        OnRespawnCompleted();

    }

    protected virtual void OnRespawnCompleted()
    {
    }

    protected void HideResourceVisuals()
    {
        ShowMeshObjects(false);
    }

    protected void ShowResourceVisuals()
    {
        ShowMeshObjects(true);
    }

    private void ShowMeshObjects(bool flag)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(flag);
        }
    }
}
