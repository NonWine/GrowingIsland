using System.Collections.Generic;
using UnityEngine;

public sealed class SawmillPileRuntime
{
    private readonly List<Transform> _stageRoots = new();

    public IReadOnlyList<Transform> StageRoots => _stageRoots;
    public int BuiltCapacity { get; private set; } = -1;

    public bool Matches(int capacity, int stageCount)
    {
        return BuiltCapacity == capacity && _stageRoots.Count == stageCount;
    }

    public void ReplaceStages(List<Transform> stageRoots, int capacity)
    {
        Clear();

        if (stageRoots != null)
            _stageRoots.AddRange(stageRoots);

        BuiltCapacity = capacity;
    }

    public void Clear()
    {
        for (int i = 0; i < _stageRoots.Count; i++)
        {
            if (_stageRoots[i] == null)
                continue;

            Object.Destroy(_stageRoots[i].gameObject);
        }

        _stageRoots.Clear();
        BuiltCapacity = -1;
    }
}
