using System;
using UnityEngine;

[Serializable]
public class DepositAnimationSettings
{
    public GameObject LogPrefab;
    public float RotationDuration = 0.12f;
    public float DelayBetweenLogs = 0.04f;

    public Vector2 AnticipationDurationRange = new(0.08f, 0.12f);
    public Vector2 ReleaseDurationRange = new(0.06f, 0.1f);
    public Vector2 FlightDurationRange = new(0.2f, 0.22f);
    public Vector2 RecoveryDurationRange = new(0.11f, 0.15f);
    public Vector2 ArcHeightRange = new(0.34f, 0.45f);
    public Vector2 SideOffsetRange = new(-0.12f, 0.12f);
    public Vector2 LandingSpreadRange = new(-0.08f, 0.08f);
    public Vector2 SpinDegreesRange = new(150f, 240f);
    public Vector2 PitchJitterRange = new(-20f, 20f);
    public Vector2 RollJitterRange = new(-24f, 24f);
    public Vector2 ImpactStrengthRange = new(0.92f, 1.08f);
    public Vector2 ApexDurationRatioRange = new(0.54f, 0.62f);

    public float ProjectileScale = 1f;
    public float ReleaseScaleMultiplier = 1.08f;
    public float ApexScaleMultiplier = 1.13f;
    public float FirstThrowTimingMultiplier = 1.08f;
    public float ComboTimingMultiplier = 0.94f;
    public int AccentEvery = 5;
    public float AccentImpactMultiplier = 1.15f;

    public Vector3 HeldLogLocalPosition = new(0.02f, 0.02f, 0.14f);
    public Vector3 HeldLogLocalEuler = new(12f, 90f, 8f);

    public ThrowVariantSettings ThrowA = ThrowVariantSettings.Create(
        heldAnticipationOffset: new Vector3(-0.03f, 0.01f, -0.04f),
        heldReleaseOffset: new Vector3(0.05f, 0.04f, 0.05f),
        heldEulerOffset: new Vector3(-8f, 4f, -10f),
        arcHeightMultiplier: 1.02f,
        sideOffsetMultiplier: 0.75f,
        spinMultiplier: 1f,
        timingMultiplier: 1f);

    public ThrowVariantSettings ThrowB = ThrowVariantSettings.Create(
        heldAnticipationOffset: new Vector3(0.05f, 0.02f, -0.05f),
        heldReleaseOffset: new Vector3(-0.04f, 0.05f, 0.08f),
        heldEulerOffset: new Vector3(10f, -14f, 16f),
        arcHeightMultiplier: 1.14f,
        sideOffsetMultiplier: 1.35f,
        spinMultiplier: 1.18f,
        timingMultiplier: 0.96f);

    public DepositAnimationSettings() { }

    public DepositAnimationSettings(DepositAnimationSettings template)
    {
        LogPrefab = template.LogPrefab;
        RotationDuration = template.RotationDuration;
        DelayBetweenLogs = template.DelayBetweenLogs;
        AnticipationDurationRange = template.AnticipationDurationRange;
        ReleaseDurationRange = template.ReleaseDurationRange;
        FlightDurationRange = template.FlightDurationRange;
        RecoveryDurationRange = template.RecoveryDurationRange;
        ArcHeightRange = template.ArcHeightRange;
        SideOffsetRange = template.SideOffsetRange;
        LandingSpreadRange = template.LandingSpreadRange;
        SpinDegreesRange = template.SpinDegreesRange;
        PitchJitterRange = template.PitchJitterRange;
        RollJitterRange = template.RollJitterRange;
        ImpactStrengthRange = template.ImpactStrengthRange;
        ApexDurationRatioRange = template.ApexDurationRatioRange;
        ProjectileScale = template.ProjectileScale;
        ReleaseScaleMultiplier = template.ReleaseScaleMultiplier;
        ApexScaleMultiplier = template.ApexScaleMultiplier;
        FirstThrowTimingMultiplier = template.FirstThrowTimingMultiplier;
        ComboTimingMultiplier = template.ComboTimingMultiplier;
        AccentEvery = template.AccentEvery;
        AccentImpactMultiplier = template.AccentImpactMultiplier;
        HeldLogLocalPosition = template.HeldLogLocalPosition;
        HeldLogLocalEuler = template.HeldLogLocalEuler;
        ThrowA = new ThrowVariantSettings(template.ThrowA);
        ThrowB = new ThrowVariantSettings(template.ThrowB);
    }
}
