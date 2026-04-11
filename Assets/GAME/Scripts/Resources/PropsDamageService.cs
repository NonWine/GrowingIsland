using UnityEngine;
using Zenject;

public class PropsDamageService : IEnvironmentResourceDamageService, IResetable, IAliveStateReader, IInitializable
{
    private readonly ResourceWorld resourceWorld;
    private readonly EnvironmentResourceEvents events;
    private float health;
    public bool IsAlive { get; private set; }

    public PropsDamageService(ResourceWorld resourceWorld, EnvironmentResourceEvents events)
    {
        this.resourceWorld = resourceWorld;
        this.events = events;
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
