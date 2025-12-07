using System;
using UnityEngine;

public class EnemyHealth
{
    private readonly EnemyStats _stats;
    private readonly HealthUI _healthUI;
    private readonly BaseEnemy _enemy;
    private readonly EnemyStateMachine _enemyStateMachine;
    private readonly IGameController _gameController;

    public event Action OnGetDamage;
    public event Action<BaseEnemy> OnDie;

    public bool IsAlive { get; set; }

    private float _timeFromGetDamage;
    public bool HasDamaged => _timeFromGetDamage < 3f;

    public EnemyHealth(EnemyStats stats,
        HealthUI healthUI,
        BaseEnemy enemy,
        IGameController gameController,
        EnemyStateMachine enemyStateMachine)
    {
        _stats = stats;
        _healthUI = healthUI;
        _enemy = enemy;
        _gameController = gameController;
        _enemyStateMachine = enemyStateMachine;

        _stats.CurrentHealth = _stats.MaxHealth;
        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        _stats.CurrentHealth -= damage;
        OnGetDamage?.Invoke();
        _healthUI.GetDamageUI(damage);

        if (_stats.CurrentHealth <= 0)
        {
            IsAlive = false;
            _enemyStateMachine.ChangeState<DieState>();
            _gameController.UnregisterFromTick(_enemy);
            OnDie?.Invoke(_enemy);
        }
    }

    public void Tick()
    {
        _timeFromGetDamage += Time.deltaTime;
    }
}
