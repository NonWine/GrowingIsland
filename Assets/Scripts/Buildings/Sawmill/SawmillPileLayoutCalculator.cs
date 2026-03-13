using UnityEngine;

public sealed class SawmillPileLayoutCalculator : ISawmillPileLayoutCalculator
{
    public int GetStageCount(SawmillPileVisualSettings settings, int capacity)
    {
        if (capacity <= 0)
            return 0;

        return Mathf.Max(1, Mathf.CeilToInt(capacity / (float)Mathf.Max(1, settings.StageSize)));
    }

    public int GetVisibleStageCount(SawmillPileVisualSettings settings, int current, int capacity, int availableStageCount)
    {
        if (current <= 0 || capacity <= 0 || availableStageCount <= 0)
            return 0;

        return Mathf.Min(availableStageCount, Mathf.CeilToInt(current / (float)Mathf.Max(1, settings.StageSize)));
    }

    public Vector3 GetStageLocalPosition(SawmillPileVisualSettings settings, int stageIndex)
    {
        return settings.BaseOffset
               + Vector3.up * (settings.StageRise * stageIndex)
               + Vector3.forward * (settings.StageDepth * stageIndex);
    }

    public Vector3 GetLogLocalPosition(SawmillPileVisualSettings settings, int stageIndex, int logIndex)
    {
        int logsPerRow = Mathf.Max(1, settings.LogsPerRow);
        int row = logIndex / logsPerRow;
        int column = logIndex % logsPerRow;
        float centeredColumn = column - (logsPerRow - 1) * 0.5f;

        return new Vector3(
            centeredColumn * settings.LogSpacing.x + Random.Range(settings.HorizontalJitter.x, settings.HorizontalJitter.y),
            row * settings.LogSpacing.y,
            Random.Range(settings.DepthJitter.x, settings.DepthJitter.y));
    }

    public Quaternion GetLogLocalRotation(SawmillPileVisualSettings settings, int stageIndex)
    {
        float baseYaw = settings.LogBaseEuler.y + (stageIndex % 2 == 0 ? 0f : 90f);
        return Quaternion.Euler(
            settings.LogBaseEuler.x,
            baseYaw + Random.Range(settings.YawJitter.x, settings.YawJitter.y),
            settings.LogBaseEuler.z);
    }
}
