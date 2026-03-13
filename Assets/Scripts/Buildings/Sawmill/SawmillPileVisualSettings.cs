using System;
using UnityEngine;

[Serializable]
public class SawmillPileVisualSettings
{
    public bool Enabled = true;
    public GameObject LogPrefab;
    public int StageSize = 5;
    public int LogsPerStage = 3;
    public int LogsPerRow = 3;
    public Vector3 BaseOffset = new(0f, 0f, 0f);
    public Vector3 LogSpacing = new(0.16f, 0.07f, 0.1f);
    public float StageRise = 0.07f;
    public float StageDepth = 0.12f;
    public Vector2 HorizontalJitter = new(-0.04f, 0.04f);
    public Vector2 DepthJitter = new(-0.04f, 0.04f);
    public Vector2 YawJitter = new(-12f, 12f);
    public Vector3 LogBaseEuler = new(0f, 90f, 0f);
    public float StagePopScale = 0.1f;
    public float StagePopDuration = 0.16f;
    public float StageSettleDuration = 0.1f;

    public SawmillPileVisualSettings() { }

    public SawmillPileVisualSettings(SawmillPileVisualSettings template)
    {
        Enabled = template.Enabled;
        LogPrefab = template.LogPrefab;
        StageSize = template.StageSize;
        LogsPerStage = template.LogsPerStage;
        LogsPerRow = template.LogsPerRow;
        BaseOffset = template.BaseOffset;
        LogSpacing = template.LogSpacing;
        StageRise = template.StageRise;
        StageDepth = template.StageDepth;
        HorizontalJitter = template.HorizontalJitter;
        DepthJitter = template.DepthJitter;
        YawJitter = template.YawJitter;
        LogBaseEuler = template.LogBaseEuler;
        StagePopScale = template.StagePopScale;
        StagePopDuration = template.StagePopDuration;
        StageSettleDuration = template.StageSettleDuration;
    }
}
