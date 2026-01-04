using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class Woodcutter : MonoBehaviour, IGameTickable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private WoodcutterWorkSettings _workSettings = new();
    [SerializeField] private LayerMask _resourceMask = ~0;
    [SerializeField] private LayerMask _resourcePartMask = ~0;
    [SerializeField] private Sawmill _sawmill;

    private IGameController _gameController;
    private OverlapSphereHandler _overlapSphereHandler;
    [ShowInInspector]  private WoodcutterContext _context;
    [ShowInInspector , HideLabel] private WoodcutterStateMachine _stateMachine;
    private bool _initialized;

    [Inject]
    private void Construct(IGameController gameController, [InjectOptional] OverlapSphereHandler overlapSphereHandler)
    {
        _gameController = gameController;
        _overlapSphereHandler = overlapSphereHandler ?? new OverlapSphereHandler();
    }

    private void Awake()
    {
        _agent ??= GetComponent<NavMeshAgent>();
        if (_sawmill != null)
        {
            _sawmill.SetWoodcutter(this);
        }
    }

    private void Start()
    {
        if (_sawmill == null)
        {
            Debug.LogError("Woodcutter has no sawmill assigned.");
            return;
        }

        BuildStateMachine();
        _gameController?.RegisterInTick(this);
        _initialized = true;
    }

    private void OnDestroy()
    {
        if (_initialized)
        {
            _gameController?.UnregisterFromTick(this);
        }
    }

    public void SetSawmill(Sawmill sawmill)
    {
        _sawmill = sawmill;
        _context?.SetSawmill(sawmill);
    }

    public void Tick()
    {
        _stateMachine?.Tick();
    }

    public bool TryFindNearestTree(out EnvironmentResource tree)
    {
        tree = null;
        if (_overlapSphereHandler == null)
            return false;

        var resources = _overlapSphereHandler.GetFilteredObjects<EnvironmentResource>(
            transform.position,
            _workSettings.TreeSearchRadius,
            _resourceMask,
            FilterTree,
            true);

        if (resources.Count == 0)
            return false;

        Transform nearest = null;
        for (int i = 0; i < resources.Count; i++)
        {
            var candidate = resources[i]?.transform;
            if (candidate == null)
                continue;

            if (nearest == null)
            {
                nearest = candidate;
                continue;
            }

            var currentSqr = (nearest.position - transform.position).sqrMagnitude;
            var candidateSqr = (candidate.position - transform.position).sqrMagnitude;
            if (candidateSqr < currentSqr)
            {
                nearest = candidate;
            }
        }
        if (nearest == null)
            return false;

        tree = nearest.GetComponent<EnvironmentResource>();
        return tree != null;
    }

    private bool FilterTree(EnvironmentResource resource)
    {
        return resource != null &&
               resource.isAlive &&
               resource.ResourceType == eCollectable.Wood;
    }

    private void BuildStateMachine()
    {
        _context = new WoodcutterContext(this, _sawmill, _agent, _animator, _workSettings, _overlapSphereHandler, _resourceMask, _resourcePartMask);
        _stateMachine = WoodcutterStateMachine.CreateDefault(_context);
    }
}

[System.Serializable]
public class WoodcutterWorkSettings
{
    public float TreeSearchRadius = 20f;
    public float ChopDistance = 1.6f;
    public float DepositDistance = 1.5f;
    public float LootCollectionRadius = 3f;
    public float DropDetectionRadius = 25f;
    public float RetargetCooldown = 0.5f;
    public float TreeDamage = 1f;
    public int WoodPerHit = 1;
}
