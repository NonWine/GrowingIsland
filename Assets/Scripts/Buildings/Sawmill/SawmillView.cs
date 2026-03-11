using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class SawmillView : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [SerializeField] private Transform _depositPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _spawnWoodcutterOnStart = true;

    [SerializeField] private UnityEvent<int, int> _storageChangedEvent;
    [SerializeField] private UnityEvent<SawmillLevelSettings> _levelChangedEvent;

    public event Action OnPlayerEntered;
    public event Action OnPlayerExited;

    public Transform DepositPoint => _depositPoint != null ? _depositPoint : transform;
    public Transform SpawnPoint => _spawnPoint != null ? _spawnPoint : transform;
    public bool SpawnWoodcutterOnStart => _spawnWoodcutterOnStart;

    public void PlayerEnter() => OnPlayerEntered?.Invoke();
    public void PlayerExit() => OnPlayerExited?.Invoke();

    [Inject]
    public void Construct(IStorage storage, SawmillUpgrader upgrader)
    {
        storage.OnStorageChanged += OnStorageChanged;
        upgrader.LevelChanged += OnLevelChanged;
    }

    private void OnStorageChanged(int current, int capacity) => _storageChangedEvent?.Invoke(current, capacity);
    private void OnLevelChanged(SawmillLevelSettings settings) => _levelChangedEvent?.Invoke(settings);
}
