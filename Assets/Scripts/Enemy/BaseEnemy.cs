using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class BaseEnemy : MonoBehaviour , IDamageable
{
    [SerializeField] protected HealthUI HealthUI;
    [SerializeField] protected NavMeshAgent NavMesh;
    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] private Stats _stats;
    [Inject] protected PlayerContainer PlayerContainer;
    protected EnemyStateMachine EnemyStateMachine;

    public Stats Stats
    {
        get => _stats;
        
        protected set => Stats = value;
    }

    public ObjectPoolEnemy ObjectPoolEnemy;
    
    public event Action<BaseEnemy> OnDie;
    public void InvokeOnDie() => OnDie?.Invoke(this);

    
    private void Awake()
    {
        Stats.OnHealthChange += HealthUI.GetDamageUI;
        Stats.OnHealthChange += Death;
    }

    private void OnDestroy()
    {
        Stats.OnHealthChange -= HealthUI.GetDamageUI;
        Stats.OnHealthChange -= Death;

    }
    
    public void Tick()
    {
        EnemyStateMachine.Update();
        HealthUI.Tick();
    }
    
    protected virtual void Death(int health)
    {
        if (health <= 0 && !Stats.IsDeath)
        {
            EnemyStateMachine.ChangeState<DieState>();
        }
    }
    
    public virtual void GetDamage(int damage)
    {
        Stats.CurrentHealth -= damage;
    }

    public abstract void ResetMob();
    
}