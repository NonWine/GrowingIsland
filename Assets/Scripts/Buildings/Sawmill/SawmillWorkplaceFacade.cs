using System;
using UnityEngine;

public class SawmillWorkplaceFacade : IWoodcutterWorkplace
{
    private readonly ISawmillView view;
    private readonly IStorage storage;

    public event Action<int, int> StorageChanged
    {
        add => storage.OnStorageChanged += value;
        remove => storage.OnStorageChanged -= value;
    }

    public event Action<float> WoodDeposited;

    public Transform DepositPoint => view.DepositPoint;
    public Vector3 WorkPlacePosition => view.WorldPosition;
    public bool IsStorageFull => storage.IsFull;
    public int StoredWood => storage.Current;

    public SawmillWorkplaceFacade(ISawmillView view, IStorage storage)
    {
        this.view = view;
        this.storage = storage;
    }

    public int DepositWood(int amount)
    {
        storage.TryStore(amount, out var stored);
        return stored;
    }

    public int DepositOneWood(float impactStrength = 1f)
    {
        storage.TryStoreSilently(1, out var stored);

        if (stored > 0)
        {
            WoodDeposited?.Invoke(impactStrength);
            storage.NotifyChanged();
        }

        return stored;
    }
}
