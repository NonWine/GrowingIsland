using System;
using UnityEngine;

public class Sawmill
{
    private readonly SawmillView _view;
    private readonly IStorage _storage;

    public event Action<int, int> StorageChanged
    {
        add => _storage.OnStorageChanged += value;
        remove => _storage.OnStorageChanged -= value;
    }

    public Transform DepositPoint => _view.DepositPoint;
    public bool IsStorageFull => _storage.IsFull;
    public int StoredWood => _storage.Current;

    public Sawmill(SawmillView view, IStorage storage)
    {
        _view = view;
        _storage = storage;
    }

    public int DepositWood(int amount)
    {
        _storage.TryStore(amount, out var stored);
        return stored;
    }
}
