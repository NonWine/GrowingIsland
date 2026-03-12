using System;
using UnityEngine;

public class SawmillFacade : IWoodcutterWorkplace
{
    private readonly SawmillView view;
    private readonly IStorage storage;

    public event Action<int, int> StorageChanged
    {
        add => storage.OnStorageChanged += value;
        remove => storage.OnStorageChanged -= value;
    }

    public event Action WoodDeposited;

    public Transform DepositPoint => view.DepositPoint;
    public Vector3 WorkPlacePosition => view.transform.position;
    public bool IsStorageFull => storage.IsFull;
    public int StoredWood => storage.Current;

    public SawmillFacade(SawmillView view, IStorage storage, SawmillUpgrader upgrader)
    {
        this.view = view;
        this.storage = storage;

        storage.OnStorageChanged += view.OnStorageChanged;
        upgrader.LevelChanged += view.OnLevelChanged;
        WoodDeposited += view.PlayReceiveAnimation;
    }

    public int DepositWood(int amount)
    {
        storage.TryStore(amount, out var stored);
        return stored;
    }

    public int DepositOneWood()
    {
        storage.TryStore(1, out var stored);
        if (stored > 0) WoodDeposited?.Invoke();
        return stored;
    }
}
