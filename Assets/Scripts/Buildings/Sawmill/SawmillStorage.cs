 using System;
using UnityEngine;
using Zenject;

public class SawmillStorage : IStorage , IInitializable
{
    public event Action<int, int> OnStorageChanged;

    public int Capacity { get; private set; }
    public int Current { get; private set; }
    public bool IsFull => Current >= Capacity;

    public SawmillStorage(int capacity)
    {
        Capacity = Mathf.Max(0, capacity);
        Current = 0;
    }

    public bool TryStore(int amount, out int stored)
        => TryStoreInternal(amount, out stored, notify: true);

    public bool TryStoreSilently(int amount, out int stored)
        => TryStoreInternal(amount, out stored, notify: false);

    public void NotifyChanged()
        => OnStorageChanged?.Invoke(Current, Capacity);

    private bool TryStoreInternal(int amount, out int stored, bool notify)
    {
        stored = 0;

        if (amount <= 0 || Capacity <= 0)
            return false;

        var spaceLeft = Mathf.Max(0, Capacity - Current);
        stored = Mathf.Min(spaceLeft, amount);

        if (stored <= 0)
            return false;

        Current += stored;
        if (notify)
            OnStorageChanged?.Invoke(Current, Capacity);

        return true;
    }

    public int TakeAll()
    {
        var result = Current;
        Current = 0;
        OnStorageChanged?.Invoke(Current, Capacity);
        return result;
    }

    public void SetCapacity(int capacity)
    {
        Capacity = Mathf.Max(0, capacity);
        Current = Mathf.Min(Current, Capacity);
        OnStorageChanged?.Invoke(Current, Capacity);
    }

    public void Initialize()
    {
        SetCapacity(Capacity);
    }
}
