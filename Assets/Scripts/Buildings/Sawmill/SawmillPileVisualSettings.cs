using System;
using UnityEngine;

[Serializable]
public class SawmillPileVisualSettings
{
    public bool Enabled = true;
    public Transform PileRoot;
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
}