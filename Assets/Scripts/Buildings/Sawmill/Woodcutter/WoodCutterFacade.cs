using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class WoodCutterFacade
{
    private readonly Sawmill _sawmill;
    private readonly WoodcutterWorkSettings _workSettings;

    [ShowInInspector, ReadOnly] public EnvironmentResource CurrentTree { get; private set; }
    [ShowInInspector, ReadOnly] public int CarriedWood { get; private set; }

    public WoodCutterFacade(Sawmill sawmill, WoodcutterWorkSettings workSettings)
    {
        _sawmill = sawmill;
        _workSettings = workSettings;
    }

    public class Factory : PlaceholderFactory<Sawmill, WoodcutterView> { }

    public Transform DepositPoint => _sawmill != null ? _sawmill.DepositPoint : null;

    public event Action<int, int> StorageChanged
    {
        add => _sawmill.StorageChanged += value;
        remove => _sawmill.StorageChanged -= value;
    }

    public bool HasTree => CurrentTree != null && CurrentTree.isAlive;
    public bool HasWood => CarriedWood > 0;
    public bool StorageFull => _sawmill != null && _sawmill.IsStorageFull;
    public int CarryCapacity => _workSettings.CarryCapacity;
    public float ChopInterval => _workSettings.ChopInterval;

    #region Actions
    public void SetTree(EnvironmentResource tree) => CurrentTree = tree;
    public void ClearTree() => CurrentTree = null;

    public void AddWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood + amount);
    public void RemoveWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood - amount);
    public void ResetWood() => CarriedWood = 0;

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
