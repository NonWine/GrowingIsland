using System;

public interface IStorage
{
    event Action<int, int> OnStorageChanged;
    int Capacity { get; }
    int Current { get; }
    bool IsFull { get; }
    bool TryStore(int amount, out int stored);
    int TakeAll();
    void SetCapacity(int capacity);
}
