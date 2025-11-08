using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Attack, Return, Stunned, Dead }
    public State state = State.Idle;

    [Header("References")]
    public Transform player;
    NavMeshAgent agent;
    Animator animator;

    [Header("Spawn / Movement")]
    public Vector3 spawnPoint;
    public float stoppingDistance = 1.8f;

    [Header("Ranges")]
    public float aggroRadius = 10f;        // коли ворог помічає гравця
    public float attackRadius = 2.2f;      // коли атакує
    public float leashRadius = 20f;        // не переступати цю відстань від spawn
    public float leashReturnExit = 17f;    // hysteresis: коли вже можна припинити повернення

    [Header("Aggro / Provocation")]
    public float aggroForgetTime = 6f;     // якщо не бачив/не був атакований — забуває
    public float provokedDuration = 8f;    // після отримання урону — не відступає цей час
    private float lastSeenTime = -999f;
    private float lastProvokedTime = -999f;
    private bool isProvoked => Time.time - lastProvokedTime <= provokedDuration;
    

    // internal
    private Transform target => player;
    private float sqrLeash => leashRadius * leashRadius;
    private float sqrAggro => aggroRadius * aggroRadius;
    private float sqrAttack => attackRadius * attackRadius;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (state == State.Dead) return;

        float sqrPlayerDist = (target.position - transform.position).sqrMagnitude;
        float sqrFromSpawnToPlayer = (target.position - spawnPoint).sqrMagnitude;

        // --- Passive aggro detection (player enters aggroRadius) ---
        if (sqrPlayerDist <= sqrAggro)
        {
            lastSeenTime = Time.time;
            if (state != State.Attack && state != State.Chase)
                ChangeState(State.Chase);
        }

        // --- State machine updates ---
        switch (state)
        {
            case State.Idle:
            case State.Patrol:
                UpdateIdlePatrol(sqrPlayerDist);
                break;
            case State.Chase:
                UpdateChase(sqrPlayerDist, sqrFromSpawnToPlayer);
                break;
            case State.Attack:
                UpdateAttack(sqrPlayerDist, sqrFromSpawnToPlayer);
                break;
            case State.Return:
                UpdateReturn();
                break;
            case State.Stunned:
                // handle stun timer/animation
                break;
        }

        // If player far away for long time and not provoked => forget
        if (!isProvoked && Time.time - lastSeenTime > aggroForgetTime && state == State.Chase)
        {
            ChangeState(State.Return);
        }
    }

    void UpdateIdlePatrol(float sqrPlayerDist)
    {
        // simple idle behavior; could patrol here
        if (sqrPlayerDist <= sqrAggro)
            ChangeState(State.Chase);
    }

    void UpdateChase(float sqrPlayerDist, float sqrFromSpawnToPlayer)
    {
        // 1) If player in attack range -> attack
        if (sqrPlayerDist <= sqrAttack)
        {
            ChangeState(State.Attack);
            return;
        }

        // 2) If player too far from spawn and NOT provoked -> return
        if (!isProvoked && sqrFromSpawnToPlayer > sqrLeash)
        {
            ChangeState(State.Return);
            return;
        }

        // 3) Continue chasing (pathfinding)
        agent.isStopped = false;
        agent.SetDestination(target.position);
        animator.SetBool("isMoving", true);
    }

    void UpdateAttack(float sqrPlayerDist, float sqrFromSpawnToPlayer)
    {
        // If player moved out of attack range -> chase
        if (sqrPlayerDist > sqrAttack * 1.1f) // small hysteresis
        {
            ChangeState(State.Chase);
            return;
        }

        // If player moved beyond leash while not provoked -> return
        if (!isProvoked && sqrFromSpawnToPlayer > sqrLeash)
        {
            ChangeState(State.Return);
            return;
        }

        // execute attack animation/logic (here simplified)
        agent.isStopped = true;
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        animator.SetTrigger("Attack");
    }

    void UpdateReturn()
    {
        float distToSpawn = Vector3.Distance(transform.position, spawnPoint);
        // If provoked while returning -> abandon return and chase immediately
        float sqrPlayerDist = (target.position - transform.position).sqrMagnitude;
        if (sqrPlayerDist <= sqrAggro || isProvoked)
        {
            ChangeState(State.Chase);
            return;
        }

        // set destination to spawn point
        agent.isStopped = false;
        agent.SetDestination(spawnPoint);
        animator.SetBool("isMoving", true);

        
    }

    // public API for taking damage / being provoked externally
    public void OnTakeDamage(float dmg, Vector3 hitPoint)
    {
        lastProvokedTime = Time.time;
        lastSeenTime = Time.time;
        // if currently returning, break return and chase
        if (state == State.Return)
            ChangeState(State.Chase);
        // play hit animation / cancel some actions
        animator.SetTrigger("Hit");
    }

    void ChangeState(State newState)
    {
        if (state == newState) return;
        // exit logic
        switch (state)
        {
            case State.Chase:
                animator.SetBool("isMoving", false);
                break;
            case State.Attack:
                break;
        }

        state = newState;

        // enter logic
        switch (newState)
        {
            case State.Idle:
                agent.isStopped = true;
                animator.SetBool("isMoving", false);
                // reset extra flags, maybe heal partially
                break;
            case State.Patrol:
                // set patrol destination
                break;
            case State.Chase:
                agent.isStopped = false;
                break;
            case State.Attack:
                agent.isStopped = true;
                break;
            case State.Return:
                agent.isStopped = false;
                agent.SetDestination(spawnPoint);
                break;
        }
    }

    // debug drawing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint, leashRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
