using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class WoodcutterContext
{
    private Sawmill _sawmill;
    private readonly Woodcutter _woodcutter;

    public WoodcutterContext(Woodcutter woodcutter,
        Sawmill sawmill,
        NavMeshAgent agent,
        Animator animator,
        WoodcutterWorkSettings workSettings,
        OverlapSphereHandler overlapSphereHandler,
        LayerMask treeMask,
        LayerMask resourcePartMask)
    {
        _woodcutter = woodcutter;
        _sawmill = sawmill;
        Agent = agent;
        Animator = animator;
        WorkSettings = workSettings;
        OverlapSphereHandler = overlapSphereHandler;
        TreeMask = treeMask;
        ResourcePartMask = resourcePartMask;
        ResourceDetector = new WoodcutterResourceDetector(this);
        NpcAnimator = new NPCAnimator(animator,1);
    }

    public NavMeshAgent Agent { get; }
    public Animator Animator { get; }
    public NPCAnimator NpcAnimator { get; private set; }
    public WoodcutterWorkSettings WorkSettings { get; }
    public OverlapSphereHandler OverlapSphereHandler { get; }
    public LayerMask TreeMask { get; }
    public LayerMask ResourcePartMask { get; }
    public WoodcutterResourceDetector ResourceDetector { get; }
    public EnvironmentResource CurrentTree { get; private set; }
    [ShowInInspector] public int CarriedWood { get; private set; }
    public Transform Transform => _woodcutter.transform;
    public Sawmill Sawmill => _sawmill;

    [ShowInInspector] public bool StorageFull => _sawmill != null && _sawmill.IsStorageFull;
    [ShowInInspector]   public int CarryCapacity => _sawmill?.CarryCapacity ?? 1;
    public float ChopInterval => _sawmill?.ChopInterval ?? 1f;
    public bool HasTree => CurrentTree != null && CurrentTree.isAlive;
    public bool HasWood => CarriedWood > 0;

    public void SetTree(EnvironmentResource tree) => CurrentTree = tree;
    public void ClearTree() => CurrentTree = null;

    public void AddWood(int amount)
    {
        CarriedWood = Mathf.Max(0, CarriedWood + amount);
    }

    public void RemoveWood(int amount)
    {
        CarriedWood = Mathf.Max(0, CarriedWood - amount);
    }

    public void ResetWood()
    {
        CarriedWood = 0;
    }

    public void SetSawmill(Sawmill sawmill)
    {
        _sawmill = sawmill;
    }

    public bool TryAcquireTree(out EnvironmentResource tree)
    {
        return _woodcutter.TryFindNearestTree(out tree);
    }
}
