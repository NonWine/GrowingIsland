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
    public Vector2 FlightDurationRange = new(0.18f, 0.22f);
    public Vector2 RecoveryDurationRange = new(0.1f, 0.15f);
    public Vector2 ArcHeightRange = new(0.25f, 0.45f);
    public Vector2 SideOffsetRange = new(-0.08f, 0.08f);
    public Vector2 LandingSpreadRange = new(-0.06f, 0.06f);
    public Vector2 SpinDegreesRange = new(120f, 220f);
    public Vector2 PitchJitterRange = new(-20f, 20f);
    public Vector2 RollJitterRange = new(-24f, 24f);
    public Vector2 ImpactStrengthRange = new(0.92f, 1.08f);
    public Vector2 ReleaseSoundPitchRange = new(0.95f, 1.05f);

    public float ProjectileScale = 1f;
    public float FirstThrowTimingMultiplier = 1.08f;
    public float ComboTimingMultiplier = 0.94f;
    public int AccentEvery = 5;
    public float AccentImpactMultiplier = 1.15f;

    public Vector3 HeldLogLocalPosition = new(0.02f, 0.02f, 0.14f);
    public Vector3 HeldLogLocalEuler = new(12f, 90f, 8f);
    public Vector3 BodyAnticipationOffset = new(0f, -0.04f, -0.08f);
    public Vector3 BodyAnticipationEuler = new(-10f, -18f, 10f);
    public Vector3 BodyReleaseOffset = new(0f, 0.05f, 0.12f);
    public Vector3 BodyReleaseEuler = new(16f, 24f, -8f);

    public ThrowVariantSettings ThrowA = ThrowVariantSettings.Create(
        "Throw_A",
        bodyOffsetScale: new Vector3(1f, 1f, 1f),
        bodyReleaseEulerOffset: new Vector3(0f, 0f, 0f),
        heldAnticipationOffset: new Vector3(-0.01f, 0f, -0.02f),
        heldReleaseOffset: new Vector3(0.02f, 0.02f, 0.02f),
        arcHeightMultiplier: 1f,
        spinMultiplier: 1f,
        timingMultiplier: 1f);

    public ThrowVariantSettings ThrowB = ThrowVariantSettings.Create(
        "Throw_B",
        bodyOffsetScale: new Vector3(1.05f, 1f, 0.92f),
        bodyReleaseEulerOffset: new Vector3(0f, -10f, 6f),
        heldAnticipationOffset: new Vector3(0.03f, 0.01f, -0.03f),
        heldReleaseOffset: new Vector3(-0.01f, 0.03f, 0.04f),
        arcHeightMultiplier: 1.08f,
        spinMultiplier: 1.12f,
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
        ProjectileScale = template.ProjectileScale;
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
    public Vector3 BodyReleaseEulerOffset = Vector3.zero;
    public Vector3 HeldAnticipationOffset = Vector3.zero;
    public Vector3 HeldReleaseOffset = Vector3.zero;
    public float ArcHeightMultiplier = 1f;
    public float SpinMultiplier = 1f;
    public float TimingMultiplier = 1f;

    public ThrowVariantSettings() { }

    public ThrowVariantSettings(ThrowVariantSettings template)
    {
        Name = template.Name;
        BodyOffsetScale = template.BodyOffsetScale;
        BodyReleaseEulerOffset = template.BodyReleaseEulerOffset;
        HeldAnticipationOffset = template.HeldAnticipationOffset;
        HeldReleaseOffset = template.HeldReleaseOffset;
        ArcHeightMultiplier = template.ArcHeightMultiplier;
        SpinMultiplier = template.SpinMultiplier;
        TimingMultiplier = template.TimingMultiplier;
    }

    public static ThrowVariantSettings Create(
        string name,
        Vector3 bodyOffsetScale,
        Vector3 bodyReleaseEulerOffset,
        Vector3 heldAnticipationOffset,
        Vector3 heldReleaseOffset,
        float arcHeightMultiplier,
        float spinMultiplier,
        float timingMultiplier)
    {
        return new ThrowVariantSettings
        {
            Name = name,
            BodyOffsetScale = bodyOffsetScale,
            BodyReleaseEulerOffset = bodyReleaseEulerOffset,
            HeldAnticipationOffset = heldAnticipationOffset,
            HeldReleaseOffset = heldReleaseOffset,
            ArcHeightMultiplier = arcHeightMultiplier,
            SpinMultiplier = spinMultiplier,
            TimingMultiplier = timingMultiplier
        };
    }
}
