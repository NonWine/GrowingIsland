using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineProperty, Serializable, HideLabel]
public class SawmillConfig
{
    [SerializeField] private List<SawmillLevelSettings> levels = new() { new SawmillLevelSettings() };
    [SerializeField, Min(0)] private int startLevelIndex;
    public int StartLevelIndex => startLevelIndex;

    public SawmillConfig(SawmillConfig sawmillConfig)
    {
        startLevelIndex = sawmillConfig.StartLevelIndex;
        levels = Levels as List<SawmillLevelSettings>;
    }
    
    public IReadOnlyList<SawmillLevelSettings> Levels => levels;

    public SawmillLevelSettings GetLevel(int index)
    {
        if (levels == null || levels.Count == 0)
            return new SawmillLevelSettings();

        if (index < 0)
            index = 0;

        if (index >= levels.Count)
            index = levels.Count - 1;

        return levels[index];
    }
}

[System.Serializable]
public class SawmillLevelSettings
{
    [Range(1, 500)] public int StorageCapacity = 25;
}
