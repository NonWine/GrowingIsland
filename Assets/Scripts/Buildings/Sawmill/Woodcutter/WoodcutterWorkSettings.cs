using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineProperty, Serializable, HideLabel]
public class WoodcutterWorkSettings
{
    [SerializeField] public LayerMask ResourceMask = ~0;
    [SerializeField] public LayerMask ResourcePartMask = ~0;
    public float TreeSearchRadius = 20f;
    public float ChopDistance = 1.6f;
    public float DepositDistance = 1.5f;
    public float LootCollectionRadius = 3f;
    public float DropDetectionRadius = 25f;
    public float RetargetCooldown = 0.5f;
    public float TreeDamage;
    public int WoodPerHit = 1;
    public float ChopInterval = 1.5f;
    public int CarryCapacity = 5;

    public DepositAnimationSettings DepositAnimation = new();

    public WoodcutterWorkSettings() { }

    public WoodcutterWorkSettings(WoodcutterWorkSettings template)
    {
        ResourceMask = template.ResourceMask;
        ResourcePartMask = template.ResourcePartMask;
        TreeSearchRadius = template.TreeSearchRadius;
        ChopDistance = template.ChopDistance;
        DepositDistance = template.DepositDistance;
        LootCollectionRadius = template.LootCollectionRadius;
        DropDetectionRadius = template.DropDetectionRadius;
        RetargetCooldown = template.RetargetCooldown;
        TreeDamage = template.TreeDamage;
        WoodPerHit = template.WoodPerHit;
        ChopInterval = template.ChopInterval;
        CarryCapacity = template.CarryCapacity;
        DepositAnimation = new DepositAnimationSettings(template.DepositAnimation);
    }
}

[Serializable]
public class DepositAnimationSettings
{
    public GameObject LogPrefab;
    public float RotationDuration = 0.12f;
    public float DelayBetweenLogs = 0.04f;
    public float LogFlightDuration = 0.2f;
    public Ease LogFlightEase = Ease.InOutSine;

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
    public Vector2 ReleaseSoundPitchRange = new(0.95f, 1.05f);
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
    public Vector3 BodyAnticipationOffset = new(0f, -0.06f, -0.14f);
    public Vector3 BodyAnticipationEuler = new(-18f, -30f, 14f);
    public Vector3 BodyReleaseOffset = new(0f, 0.09f, 0.22f);
    public Vector3 BodyReleaseEuler = new(28f, 40f, -14f);

    public ThrowVariantSettings ThrowA = ThrowVariantSettings.Create(
        "Throw_A",
        bodyOffsetScale: new Vector3(1f, 1f, 1f),
        bodyAnticipationEulerOffset: new Vector3(0f, -8f, 4f),
        bodyReleaseEulerOffset: new Vector3(0f, 12f, -2f),
        heldAnticipationOffset: new Vector3(-0.03f, 0.01f, -0.04f),
        heldReleaseOffset: new Vector3(0.05f, 0.04f, 0.05f),
        heldEulerOffset: new Vector3(-8f, 4f, -10f),
        arcHeightMultiplier: 1.02f,
        sideOffsetMultiplier: 0.75f,
        spinMultiplier: 1f,
        timingMultiplier: 1f);

    public ThrowVariantSettings ThrowB = ThrowVariantSettings.Create(
        "Throw_B",
        bodyOffsetScale: new Vector3(1.16f, 1f, 0.9f),
        bodyAnticipationEulerOffset: new Vector3(2f, 16f, -12f),
        bodyReleaseEulerOffset: new Vector3(4f, -22f, 10f),
        heldAnticipationOffset: new Vector3(0.05f, 0.02f, -0.05f),
        heldReleaseOffset: new Vector3(-0.04f, 0.05f, 0.08f),
        heldEulerOffset: new Vector3(10f, -14f, 16f),
        arcHeightMultiplier: 1.14f,
        sideOffsetMultiplier: 1.35f,
        spinMultiplier: 1.18f,
        timingMultiplier: 0.96f);

    public AudioClip[] ReleaseClips;
    public float ReleaseVolume = 1f;

    public DepositAnimationSettings() { }

    public DepositAnimationSettings(DepositAnimationSettings template)
    {
        LogPrefab = template.LogPrefab;
        RotationDuration = template.RotationDuration;
        DelayBetweenLogs = template.DelayBetweenLogs;
        LogFlightDuration = template.LogFlightDuration;
        LogFlightEase = template.LogFlightEase;
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
        ReleaseSoundPitchRange = template.ReleaseSoundPitchRange;
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
        BodyAnticipationOffset = template.BodyAnticipationOffset;
        BodyAnticipationEuler = template.BodyAnticipationEuler;
        BodyReleaseOffset = template.BodyReleaseOffset;
        BodyReleaseEuler = template.BodyReleaseEuler;
        ThrowA = new ThrowVariantSettings(template.ThrowA);
        ThrowB = new ThrowVariantSettings(template.ThrowB);
        ReleaseClips = template.ReleaseClips;
        ReleaseVolume = template.ReleaseVolume;
    }
}

[Serializable]
public class ThrowVariantSettings
{
    public string Name = "Throw";
    public Vector3 BodyOffsetScale = Vector3.one;
    public Vector3 BodyAnticipationEulerOffset = Vector3.zero;
    public Vector3 BodyReleaseEulerOffset = Vector3.zero;
    public Vector3 HeldAnticipationOffset = Vector3.zero;
    public Vector3 HeldReleaseOffset = Vector3.zero;
    public Vector3 HeldEulerOffset = Vector3.zero;
    public float ArcHeightMultiplier = 1f;
    public float SideOffsetMultiplier = 1f;
    public float SpinMultiplier = 1f;
    public float TimingMultiplier = 1f;

    public ThrowVariantSettings() { }

    public ThrowVariantSettings(ThrowVariantSettings template)
    {
        Name = template.Name;
        BodyOffsetScale = template.BodyOffsetScale;
        BodyAnticipationEulerOffset = template.BodyAnticipationEulerOffset;
        BodyReleaseEulerOffset = template.BodyReleaseEulerOffset;
        HeldAnticipationOffset = template.HeldAnticipationOffset;
        HeldReleaseOffset = template.HeldReleaseOffset;
        HeldEulerOffset = template.HeldEulerOffset;
        ArcHeightMultiplier = template.ArcHeightMultiplier;
        SideOffsetMultiplier = template.SideOffsetMultiplier;
        SpinMultiplier = template.SpinMultiplier;
        TimingMultiplier = template.TimingMultiplier;
    }

    public static ThrowVariantSettings Create(
        string name,
        Vector3 bodyOffsetScale,
        Vector3 bodyAnticipationEulerOffset,
        Vector3 bodyReleaseEulerOffset,
        Vector3 heldAnticipationOffset,
        Vector3 heldReleaseOffset,
        Vector3 heldEulerOffset,
        float arcHeightMultiplier,
        float sideOffsetMultiplier,
        float spinMultiplier,
        float timingMultiplier)
    {
        return new ThrowVariantSettings
        {
            Name = name,
            BodyOffsetScale = bodyOffsetScale,
            BodyAnticipationEulerOffset = bodyAnticipationEulerOffset,
            BodyReleaseEulerOffset = bodyReleaseEulerOffset,
            HeldAnticipationOffset = heldAnticipationOffset,
            HeldReleaseOffset = heldReleaseOffset,
            HeldEulerOffset = heldEulerOffset,
            ArcHeightMultiplier = arcHeightMultiplier,
            SideOffsetMultiplier = sideOffsetMultiplier,
            SpinMultiplier = spinMultiplier,
            TimingMultiplier = timingMultiplier
        };
    }
}
