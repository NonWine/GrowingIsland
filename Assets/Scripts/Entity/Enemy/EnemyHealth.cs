using System;
using UnityEngine;

public class EnemyHealth
{
    private EnemyStats _stats;
    private HealthUI _healthUI;
    private BaseEnemy _enemy;
    private EnemyStateMachine _enemyStateMachine;
    private IGameСontroller _gameСontroller;
    
    public event Action OnGetDamage;
    public event Action<BaseEnemy> OnDie;
    
    public bool IsAlive { get; set; }
    
    private float _timeFromGetDamage;
    public bool HasDamaged => _timeFromGetDamage < 3f;



    public EnemyHealth(EnemyStats stats, HealthUI healthUI, BaseEnemy enemy, IGameСontroller gameСontroller, EnemyStateMachine enemyStateMachine)
    {
        _stats = stats;
        _healthUI = healthUI;
        _gameСontroller = gameСontroller;
        _enemy = enemy;
        this._enemyStateMachine = enemyStateMachine;
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
            _gameСontroller.UnregisterFromTick(_enemy);
            OnDie?.Invoke(_enemy);
        }
    }

    public void Tick()
    {
        _timeFromGetDamage += Time.deltaTime;
    }
}