using System;
using UnityEngine;

public class EnemyHealth
{
    private readonly EnemyStats stats;
    private readonly HealthUI healthUI;
    private readonly BaseEnemy enemy;
    private readonly EnemyStateMachine enemyStateMachine;
    private readonly IGameController gameController;

    public event Action OnGetDamage;
    public event Action<BaseEnemy> OnDie;

    public bool IsAlive { get; set; }

    private float timeFromGetDamage;
    public bool HasDamaged => timeFromGetDamage < 3f;

    public EnemyHealth(EnemyStats stats,
        HealthUI healthUI,
        BaseEnemy enemy,
        IGameController gameController,
        EnemyStateMachine enemyStateMachine)
    {
        this.stats = stats;
        this.healthUI = healthUI;
        this.enemy = enemy;
        this.gameController = gameController;
        this.enemyStateMachine = enemyStateMachine;

        stats.CurrentHealth = stats.MaxHealth;
        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        stats.CurrentHealth -= damage;
        OnGetDamage?.Invoke();
        healthUI.GetDamageUI(damage);

        if (stats.CurrentHealth <= 0)
        {
            IsAlive = false;
            enemyStateMachine.ChangeState<DieState>();
            gameController.UnregisterFromTick(enemy);
            OnDie?.Invoke(enemy);
        }
    }

    public void Tick()
    {
        timeFromGetDamage += Time.deltaTime;
    }
}

