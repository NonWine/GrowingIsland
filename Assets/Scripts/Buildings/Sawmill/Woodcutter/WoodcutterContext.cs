using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

/// <summary>
/// Контекст дроворуба, що містить спільні дані та інструменти для станів стейт-машини.
/// </summary>
[System.Serializable]
public class WoodcutterContext
{
    private Sawmill _sawmill;
    private readonly Transform _transform;

    [Inject]
    public WoodcutterContext(
        Transform transform,
        Sawmill sawmill,
        NavMeshAgent agent,
        NPCAnimator npcAnimator,
        WoodcutterWorkSettings workSettings,
        WoodcutterSensor sensor)
    {
        _transform = transform;
        _sawmill = sawmill;
        Agent = agent;
        NpcAnimator = npcAnimator;
        WorkSettings = workSettings;
        Sensor = sensor;
    }

    #region Components & Tools
    public NavMeshAgent Agent { get; }
    public NPCAnimator NpcAnimator { get; }
    public WoodcutterWorkSettings WorkSettings { get; }
    public WoodcutterSensor Sensor { get; }
    public Transform Transform => _transform;
    public Sawmill Sawmill => _sawmill;
    #endregion

    #region State Data
    [ShowInInspector, ReadOnly] public EnvironmentResource CurrentTree { get; private set; }
    [ShowInInspector, ReadOnly] public int CarriedWood { get; private set; }
    #endregion

    #region Properties
    public bool StorageFull => _sawmill != null && _sawmill.IsStorageFull;
    public int CarryCapacity => _sawmill?.CarryCapacity ?? 1;
    public float ChopInterval => _sawmill?.ChopInterval ?? 1f;
    public bool HasTree => CurrentTree != null && CurrentTree.isAlive;
    public bool HasWood => CarriedWood > 0;
    #endregion

    #region Actions
    public void SetTree(EnvironmentResource tree) => CurrentTree = tree;
    public void ClearTree() => CurrentTree = null;

    public void AddWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood + amount);
    public void RemoveWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood - amount);
    public void ResetWood() => CarriedWood = 0;

    public void SetSawmill(Sawmill sawmill) => _sawmill = sawmill;

    public void TryDepositWood()
    {
        if (HasWood && _sawmill != null)
        {
            var stored = _sawmill.DepositWood(CarriedWood);
            RemoveWood(stored);
        }
    }
    #endregion
}
