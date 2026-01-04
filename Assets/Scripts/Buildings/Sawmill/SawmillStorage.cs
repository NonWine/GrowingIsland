using System;
using UnityEngine;

public class SawmillStorage
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
    {
        stored = 0;

        if (amount <= 0 || Capacity <= 0)
            return false;

        var spaceLeft = Mathf.Max(0, Capacity - Current);
        stored = Mathf.Min(spaceLeft, amount);

        if (stored <= 0)
            return false;

        Current += stored;
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
}
