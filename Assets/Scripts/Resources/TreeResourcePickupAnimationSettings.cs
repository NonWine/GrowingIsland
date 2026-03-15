using System;
using UnityEngine;

[Serializable]
public class TreeResourcePickupAnimationSettings
{
    [Min(1)] public int SpawnCountMin = 2;
    [Min(1)] public int SpawnCountMax = 4;

    public ResourcePartObj PiecePrefabOverride;

    [Min(0.01f)] public float LaunchDurationMin = 0.2f;
    [Min(0.01f)] public float LaunchDurationMax = 0.28f;
    [Min(0.01f)] public float SettleDurationMin = 0.06f;
    [Min(0.01f)] public float SettleDurationMax = 0.11f;
    [Min(0f)] public float PickupDelayMin = 0.08f;
    [Min(0f)] public float PickupDelayMax = 0.16f;
    [Min(0.01f)] public float PickupFlightDurationMin = 0.2f;
    [Min(0.01f)] public float PickupFlightDurationMax = 0.28f;

    [Min(0f)] public float LaunchArcHeightMin = 0.18f;
    [Min(0f)] public float LaunchArcHeightMax = 0.3f;
    [Min(0f)] public float PickupArcHeightMin = 0.08f;
    [Min(0f)] public float PickupArcHeightMax = 0.16f;
    [Min(0f)] public float SideOffset = 0.22f;
    [Min(0f)] public float LandingHeightOffset = 0.08f;
    [Min(0f)] public float RandomYawOffset = 16f;
    [Min(0f)] public float RandomPitchOffset = 8f;
    [Min(0f)] public float RandomRollOffset = 12f;

    [Range(0f, 0.25f)] public float LandBounceHeight = 0.04f;
    [Range(0.8f, 1.2f)] public float SettleSquashScale = 0.92f;
    [Range(1f, 1.3f)] public float FinalPickupPopScale = 1.12f;
    [Min(0.01f)] public float FinalPickupPopDuration = 0.08f;

    public TreeResourcePickupAnimationSettings() { }

    public TreeResourcePickupAnimationSettings(TreeResourcePickupAnimationSettings template)
    {
        if (template == null)
            return;

        SpawnCountMin = template.SpawnCountMin;
        SpawnCountMax = template.SpawnCountMax;
        PiecePrefabOverride = template.PiecePrefabOverride;
        LaunchDurationMin = template.LaunchDurationMin;
        LaunchDurationMax = template.LaunchDurationMax;
        SettleDurationMin = template.SettleDurationMin;
        SettleDurationMax = template.SettleDurationMax;
        PickupDelayMin = template.PickupDelayMin;
        PickupDelayMax = template.PickupDelayMax;
        PickupFlightDurationMin = template.PickupFlightDurationMin;
        PickupFlightDurationMax = template.PickupFlightDurationMax;
        LaunchArcHeightMin = template.LaunchArcHeightMin;
        LaunchArcHeightMax = template.LaunchArcHeightMax;
        PickupArcHeightMin = template.PickupArcHeightMin;
        PickupArcHeightMax = template.PickupArcHeightMax;
        SideOffset = template.SideOffset;
        LandingHeightOffset = template.LandingHeightOffset;
        RandomYawOffset = template.RandomYawOffset;
        RandomPitchOffset = template.RandomPitchOffset;
        RandomRollOffset = template.RandomRollOffset;
        LandBounceHeight = template.LandBounceHeight;
        SettleSquashScale = template.SettleSquashScale;
        FinalPickupPopScale = template.FinalPickupPopScale;
        FinalPickupPopDuration = template.FinalPickupPopDuration;
    }

    public int GetSpawnCount()
    {
        int min = Mathf.Max(1, SpawnCountMin);
        int max = Mathf.Max(min, SpawnCountMax);
        return UnityEngine.Random.Range(min, max + 1);
    }
}
