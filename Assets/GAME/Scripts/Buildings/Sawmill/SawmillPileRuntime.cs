using System.Collections.Generic;
using UnityEngine;

public sealed class SawmillPileRuntime
{
    private readonly List<Transform> stageRoots = new();

    public IReadOnlyList<Transform> StageRoots => stageRoots;
    public int BuiltCapacity { get; private set; } = -1;

    public bool Matches(int capacity, int stageCount)
    {
        return BuiltCapacity == capacity && stageRoots.Count == stageCount;
    }

    public void ReplaceStages(List<Transform> stageRoots, int capacity)
    {
        Clear();

        if (stageRoots != null)
            stageRoots.AddRange(stageRoots);

        BuiltCapacity = capacity;
    }

    public void Clear()
    {
        for (int i = 0; i < stageRoots.Count; i++)
        {
            if (stageRoots[i] == null)
                continue;

            Object.Destroy(stageRoots[i].gameObject);
        }

        stageRoots.Clear();
        BuiltCapacity = -1;
    }
}
