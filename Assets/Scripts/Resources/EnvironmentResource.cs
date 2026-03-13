using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

public abstract class EnvironmentResource : MonoBehaviour , IDamageable
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
    
    

    protected virtual void AnimTrigDamage()
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
        if(!isAlive)
            return;
        
        health -= damage;
        AnimTrigDamage();
        if (health <= 0)
        {
            StartCoroutine(RespawmProp());
        }
    }

    private void SpawnResources()
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

    private IEnumerator RespawmProp()
    {
        SpawnResources();
        isAlive = false;
        ShowMeshObjects(false);
        yield return new WaitForSeconds(respawnTime);
        isAlive = true;
        health = resourceWorld.Health;
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
