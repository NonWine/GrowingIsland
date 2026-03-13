using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class WoodCutterFacade : IDisposable , IInitializable
{
    private readonly IWoodcutterWorkplace workplace;
    private readonly WoodcutterWorkSettings workSettings;
    private readonly Action<int, int> _storageChangedRelay;

    [ShowInInspector, ReadOnly] public EnvironmentResource CurrentTree { get; private set; }
    [ShowInInspector, ReadOnly] public int CarriedWood { get; private set; }

    public WoodCutterFacade(IWoodcutterWorkplace workplace, WoodcutterWorkSettings workSettings)
    {
        this.workplace = workplace;
        this.workSettings = workSettings;
        _storageChangedRelay = (current, capacity) => StorageChanged?.Invoke(current, capacity);
    }

    public class Factory : PlaceholderFactory<IWoodcutterWorkplace, WoodcutterView> { }

    public Transform DepositPoint => workplace?.DepositPoint;
    public Vector3 WorkPlacePosition => workplace.WorkPlacePosition;

    public event Action<int, int> StorageChanged;

    public bool HasTree =>  CurrentTree.isAlive;
    public bool HasWood => CarriedWood > 0;
    public bool WorkPlaceStorageFull =>  workplace.IsStorageFull;

    #region Actions
    public void SetTree(EnvironmentResource tree) => CurrentTree = tree;
    public void ClearTree() => CurrentTree = null;

    public void AddWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood + amount);
    public void RemoveWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood - amount);
    public void ResetWood() => CarriedWood = 0;

    public void DepositOneWood(float impactStrength = 1f)
    {
        var stored = workplace.DepositOneWood(impactStrength);
        if (stored > 0) RemoveWood(stored);
    }
    #endregion

    public void Dispose()
    {
        workplace.StorageChanged -= _storageChangedRelay;
    }

    public void Initialize()
    {
        workplace.StorageChanged += _storageChangedRelay;
    }
}
