using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public abstract class EnvironmentResource : MonoBehaviour , IDamageable
{
    [SerializeField] private ResourceWorld _resourceWorld;
    [SerializeField] private float _respawnTime = 10f;
    [Inject] private ResourcePartObjFactory _resourcesFactory;
    private float _health;

    public eCollectable ResourceType => _resourceWorld.TypeWallet;
    
    
    private void Awake()
    {
        isAlive = true;
        _health = _resourceWorld.Health;
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
        
        _health -= damage;
        AnimTrigDamage();
        if (_health <= 0)
        {
            SpawnResources();
            StartCoroutine(RespawmProp());

        }
    }

    private void SpawnResources()
    {
        for (int i = 0; i < _resourceWorld.VisualDrop; i++)
        {
            var resource = _resourcesFactory.Create(_resourceWorld.TypeWallet);
            resource.transform.position = transform.position;
            var offset = transform.position + Random.insideUnitSphere * 2f;
            offset.y = resource.transform.position.y;
            resource.transform.DOMove(offset, 0.8f).OnComplete(resource.SetIdle).SetEase(Ease.OutQuart);
            resource.ResetPool();
        }
    }

    public bool isAlive { get; set; }

    private IEnumerator RespawmProp()
    {
        ShowMeshObjects(false);
        isAlive = false;
        yield return new WaitForSeconds(_respawnTime);
        isAlive = true;
        _health = _resourceWorld.Health;
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