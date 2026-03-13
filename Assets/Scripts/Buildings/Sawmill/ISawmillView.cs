using System;
using UnityEngine;

public interface ISawmillView
{
    event Action OnPlayerEntered;
    event Action OnPlayerExited;

    Transform DepositPoint { get; }
    Transform SpawnPoint { get; }
    Vector3 WorldPosition { get; }
    bool SpawnWoodcutterOnStart { get; }

    void RenderStorage(int current, int capacity, bool isFull);
    void NotifyStorageChanged(int current, int capacity);
    void NotifyLevelChanged(SawmillLevelSettings settings);
    void NotifyDepositImpact();
}
