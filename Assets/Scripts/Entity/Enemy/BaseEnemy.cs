using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class BaseEnemy : PoolAble , IDamageable , IGameTickable
{
    [SerializeField] private EnemyStats _stats;
    [SerializeField] protected HealthUI HealthUI;
    [SerializeField] protected NavMeshAgent NavMesh;
    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] protected Animator Animator;
    [Inject] protected Player Player;
    [Inject] protected IGameСontroller GameСontroller;
    protected EnemyStateMachine EnemyStateMachine;
    private float _timeFromGetDamage;
    
    public EnemyStats Stats
    {
        get => _stats;
        
        protected set => Stats = value;
    }

   // public ObjectPoolEnemy ObjectPoolEnemy;
    
    public event Action<BaseEnemy> OnDie;

    public event Action OnGetDamage;

    public event Action OnDrawGizmoz;

    public bool HasDamaged => _timeFromGetDamage < 3f;
    
    

    
    private void Awake()
    {
        NavMesh.speed = _stats.MoveSpeed;
        
        GameСontroller.RegisterInTick(this);
        
        isAlive = true;
    }
    
    public void Tick()
    {
        EnemyStateMachine.Update();
        HealthUI.Tick();

        _timeFromGetDamage += Time.deltaTime;
        
    }
    
    public virtual void GetDamage(float damage)
    {
        Stats.CurrentHealth -= damage;
        _timeFromGetDamage = 0f;
        OnGetDamage?.Invoke();
        HealthUI.GetDamageUI(damage);
        if (_stats.CurrentHealth <= 0)
        {
            EnemyStateMachine.ChangeState<DieState>();
            GameСontroller.UnregisterFromTick(this);

        }
    }

    public void SetTargetPlayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized; // Отримуємо напрямок
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * EnemyStateMachine.Enemy.Stats.RotateSpeed);
    }

    public bool isAlive { get; set; }

    public virtual void ResetMob()
    {
        EnemyStateMachine.ChangeState<ResetingState>();
    }

    private void OnDrawGizmos()
    {
        OnDrawGizmoz?.Invoke();
    }
    
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if(Player == null)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, Player.transform.position);

        // Вычисляем расстояние
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // Показываем текст с расстоянием
        UnityEditor.Handles.color = Color.black;
        UnityEditor.Handles.Label((transform.position + Player.transform.position) / 2, distance.ToString("F2"));
    }
    
#endif
}