using System;
using UnityEngine;

public class SawmillUpgrader
{
    private readonly SawmillConfig config;
    private readonly IStorage storage;

    private int currentLevelIndex;

    public event Action<SawmillLevelSettings> LevelChanged;

    public SawmillLevelSettings CurrentLevel => config.GetLevel(currentLevelIndex);

    public SawmillUpgrader(SawmillConfig config, IStorage storage)
    {
        this.config = config;
        this.storage = storage;
        currentLevelIndex = Mathf.Max(0, config.StartLevelIndex);
    }

    public void NotifyInitial() => LevelChanged?.Invoke(CurrentLevel);

    public void Upgrade()
    {
        if (config.Levels == null || currentLevelIndex >= config.Levels.Count - 1)
            return;

        currentLevelIndex++;
        storage.SetCapacity(CurrentLevel.StorageCapacity);
        LevelChanged?.Invoke(CurrentLevel);
    }
}
