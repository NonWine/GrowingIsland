using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Sawmill/SawmillConfig", fileName = "SawmillConfig", order = 0)]
public class SawmillConfig : ScriptableObject
{
    [SerializeField] private List<SawmillLevelSettings> _levels = new() { new SawmillLevelSettings() };

    public IReadOnlyList<SawmillLevelSettings> Levels => _levels;

    public SawmillLevelSettings GetLevel(int index)
    {
        if (_levels == null || _levels.Count == 0)
            return new SawmillLevelSettings();

        if (index < 0)
            index = 0;

        if (index >= _levels.Count)
            index = _levels.Count - 1;

        return _levels[index];
    }
}

[System.Serializable]
public class SawmillLevelSettings
{
    [Range(1, 500)] public int StorageCapacity = 25;
    [Range(0.1f, 10f)] public float ChopInterval = 1.5f;
    [Range(1, 50)] public int CarryCapacity = 5;
}
