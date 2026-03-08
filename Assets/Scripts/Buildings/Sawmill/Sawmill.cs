using System;
using UnityEngine;
using Zenject;

public class Sawmill : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [SerializeField] private SawmillConfig _config;
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private Transform woodCutterSpawnPoint;
    [Header("Woodcutter Spawn")]
    [SerializeField] private bool _spawnWoodcutterOnStart = true;

    [Inject] private CollectableManager _collectableManager;
    [Inject] private WoodCutterFacade.Factory _woodcutterFactory;

    private SawmillStorage _storage;
    private int _currentLevelIndex;

    public event Action<int, int> StorageChanged;
    public event Action<SawmillLevelSettings> LevelChanged;

    public Transform DepositPoint => _dropPoint == null ? transform : _dropPoint;
    public bool IsStorageFull => _storage != null && _storage.IsFull;
    public int StoredWood => _storage?.Current ?? 0;

    public SawmillLevelSettings CurrentLevel => _config != null
        ? _config.GetLevel(_currentLevelIndex)
        : new SawmillLevelSettings();

    public float ChopInterval => CurrentLevel.ChopInterval;
    public int CarryCapacity => CurrentLevel.CarryCapacity;

    private void Awake()
    {
        _currentLevelIndex = Mathf.Max(0, _config.StartLevelIndex);
        _storage = new SawmillStorage(CurrentLevel.StorageCapacity);
        _storage.OnStorageChanged += OnStorageChangedInternal;
    }

    private void Start()
    {
        NotifyLevelChanged();
        
        if (_spawnWoodcutterOnStart)
        {
            SpawnWoodcutter();
        }
    }

    private void OnDestroy()
    {
        if (_storage != null)
        {
            _storage.OnStorageChanged -= OnStorageChangedInternal;
        }
    }

    public void SpawnWoodcutter()
    {
      var woodcutter =  _woodcutterFactory.Create(this);
      woodcutter.transform.position = woodCutterSpawnPoint.position;
    }

    public int DepositWood(int amount)
    {
        if (_storage == null)
            return 0;

        _storage.TryStore(amount, out var stored);
        return stored;
    }

    public void PlayerEnter()
    {
        GiveAllToPlayer();
    }

    public void PlayerExit()
    {
    }

    public void Upgrade()
    {
        if (_config == null || _config.Levels == null || _config.Levels.Count == 0)
            return;

        if (_currentLevelIndex >= _config.Levels.Count - 1)
            return;

        _currentLevelIndex++;
        _storage.SetCapacity(CurrentLevel.StorageCapacity);
        NotifyLevelChanged();
    }

    private void GiveAllToPlayer()
    {
        if (_storage == null)
            return;

        var amount = _storage.TakeAll();
        if (amount <= 0)
            return;

        if (_collectableManager == null)
            return;

        var wallet = _collectableManager.GetWallet(eCollectable.Wood);
        wallet?.Add(amount);
    }

    private void OnStorageChangedInternal(int current, int capacity)
    {
        StorageChanged?.Invoke(current, capacity);
    }

    private void NotifyLevelChanged()
    {
        LevelChanged?.Invoke(CurrentLevel);
    }
}
