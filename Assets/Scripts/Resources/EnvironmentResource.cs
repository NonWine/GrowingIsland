using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public abstract class EnvironmentResource : MonoBehaviour , IDamageable
{
    [SerializeField] private ResourceWorld _resourceWorld;
    [Inject] private ResourcePartObjFactory _resourcesFactory;
    private float _health;
    
    
    
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
        _health -= damage;
        AnimTrigDamage();
        if (_health <= 0)
        {
            for (int i = 0; i < _resourceWorld.VisualDrop; i++)
            {
                var resource = _resourcesFactory.Create(_resourceWorld.TypeWallet);
                resource.transform.position = transform.position;
                var offset = transform.position + Random.insideUnitSphere * 2f;
                offset.y = resource.transform.position.y;
                resource.transform.DOMove(offset, 1.5f).OnComplete(resource.ResetPool);

            }

            isAlive = false;
            gameObject.SetActive(false);
            
        }
    }

    public bool isAlive { get; set; }
}