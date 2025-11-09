using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class BaseEnemy : PoolAble , IDamageable , IGameTickable
{
    [field: SerializeField, ReadOnly, InjectOptional] public EnemyStats Stats { get; private set; }
    [SerializeField] protected HealthUI HealthUI;
    [SerializeField] protected NavMeshAgent NavMesh;
    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] protected Animator Animator;
    [Inject] protected IGameСontroller GameСontroller;
    [ShowInInspector, ReadOnly] protected EnemyStateMachine EnemyStateMachine;
    [Inject] public Player Player { get; private set; }
    public  EnemyAnimator EnemyAnimator { get; private set; }
    public  EnemyRotator EnemyRotator { get; private set; }
    public  EnemyHealth EnemyHealth { get; private set; }
    
    private bool _isDeath;
    private Vector3 spawnPoint;

    public bool IsDeath
    {
        get => _isDeath;
        set => _isDeath = value;
    }
    
    private void Awake()
    {
        NavMesh.speed = Stats.MoveSpeed;
        Stats.CurrentHealth = Stats.MaxHealth;
        HealthUI.SetHealth(Stats.CurrentHealth);
        GameСontroller.RegisterInTick(this);
    }

    private void Start()
    {
        spawnPoint = transform.position;
        CreateComponents();
        NavMesh.updateRotation = false;
        EnemyStateMachine = new EnemyStateMachine(this);
        EnemyStateMachine.Initialize<EnemyIdleState>(CreateStates());
        EnemyHealth = new EnemyHealth(Stats, HealthUI, this, GameСontroller , EnemyStateMachine);

    }

    public void Tick()
    {
        EnemyStateMachine.Update();
        HealthUI.Tick();
        EnemyHealth.Tick();
    }
    
    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, Player.transform.position) 
               <= range;
    }
    
    public void OnProvoked()
    {
        // Якщо ворог відступає, але отримав урон — переключаємось у Chase

        if (EnemyStateMachine.CurrentState.GetType() == typeof(EnemyBackHomeState))
        {
            EnemyStateMachine.ChangeState<ChaseState>();
        }
    }
    
    public override void ResetPool()
    {
        EnemyStateMachine.ChangeState<ResetingState>();
    }

    protected virtual void CreateComponents()
    {
        EnemyAnimator = new EnemyAnimator(Animator);
        EnemyRotator = new EnemyRotator(transform, Player.transform,spawnPoint ,Stats);
    }
    
    protected virtual Dictionary<Type, EnemyState> CreateStates()
    {
        return new Dictionary<Type, EnemyState>
        {
            { typeof(EnemyIdleState), new EnemyIdleState(EnemyAnimator, EnemyStateMachine) },
            { typeof(AttackState), new AttackState(EnemyAnimator, EnemyStateMachine, new EnemyAttackIdle(Player, EnemyStateMachine)) },
            { typeof(ChaseState), new ChaseState(EnemyStateMachine, EnemyAnimator, new EnemyMovingIdle(transform.position, NavMesh, Player.PlayerContainer, EnemyStateMachine)) },
            { typeof(DieState), new DieState(EnemyAnimator, EnemyStateMachine, NavMesh) },
            { typeof(ResetingState), new ResetingState(EnemyStateMachine, EnemyAnimator, NavMesh, Rigidbody, HealthUI) },
            { typeof(EnemyBackHomeState), new EnemyBackHomeState(EnemyAnimator, EnemyStateMachine, Player, NavMesh, spawnPoint) }
        };
    }
    
    public void GetDamage(float damage) => EnemyHealth.TakeDamage(damage);

    public bool isAlive
    {
        get => EnemyHealth.IsAlive;
        set => EnemyHealth.IsAlive = value;
    }
    

    #region Editor

    public event Action OnDrawGizmoz;

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            OnDrawGizmoz?.Invoke();
        }
        

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

    #endregion
}

