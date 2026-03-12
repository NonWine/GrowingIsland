using System;
using UnityEngine;

public interface IWoodcutterWorkplace
{
    Transform DepositPoint { get; }
    Vector3 WorkPlacePosition { get; }
    bool IsStorageFull { get; }
    event Action<int, int> StorageChanged;
    event Action WoodDeposited;
    int DepositWood(int amount);
    int DepositOneWood();
}
