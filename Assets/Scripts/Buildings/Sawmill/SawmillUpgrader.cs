using System;
using UnityEngine;

public class SawmillUpgrader
{
    private readonly SawmillConfig _config;
    private readonly IStorage _storage;

    private int _currentLevelIndex;

    public event Action<SawmillLevelSettings> LevelChanged;

    public SawmillLevelSettings CurrentLevel => _config.GetLevel(_currentLevelIndex);

    public SawmillUpgrader(SawmillConfig config, IStorage storage)
    {
        _config = config;
        _storage = storage;
        _currentLevelIndex = Mathf.Max(0, _config.StartLevelIndex);
    }

    public void NotifyInitial() => LevelChanged?.Invoke(CurrentLevel);

    public void Upgrade()
    {
        if (_config.Levels == null || _currentLevelIndex >= _config.Levels.Count - 1)
            return;

        _currentLevelIndex++;
        _storage.SetCapacity(CurrentLevel.StorageCapacity);
        LevelChanged?.Invoke(CurrentLevel);
    }
}
