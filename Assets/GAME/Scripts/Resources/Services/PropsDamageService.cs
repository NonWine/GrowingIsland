using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[Serializable]
public class PropsDamageService : IEnvPropDamageService, IResetable, IAliveStateReader, IInitializable
{
    private readonly ResourceWorld resourceWorld;
 
    [SerializeField, ReadOnly] private float health;
    [ShowInInspector] public bool IsAlive { get; private set; }

    public PropsDamageService(ResourceWorld resourceWorld)
    {
        this.resourceWorld = resourceWorld;
    }


    public void Initialize()
    {
        Reset();
    }

    public void Reset()
    {
        IsAlive = true;
        health = resourceWorld.Health;
    }

    public EnvironmentResourceHitResult ApplyDamage(float damage, Vector3 sourceWorldPosition)
    {

        if (!IsAlive)
        {
            return new EnvironmentResourceHitResult(damage, 0f,false,false,sourceWorldPosition);
        }

        health -= damage;
        bool isFinalHit = health <= 0f;
     
        if (isFinalHit)
        {
            health = 0f;
            IsAlive = false;
        }
        
        return new EnvironmentResourceHitResult(damage, health, isFinalHit, true,sourceWorldPosition);
    }
}
