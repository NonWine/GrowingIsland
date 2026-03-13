using UnityEngine;

public sealed class WoodcutterDepositPlanBuilder : IWoodcutterDepositPlanBuilder
{
    public WoodcutterDepositThrowPlan Build(int throwNumber, bool startWithVariantB, DepositAnimationSettings settings)
    {
        ThrowVariantSettings variant = SelectVariant(settings, throwNumber, startWithVariantB);
        float timingMultiplier = throwNumber == 0
            ? settings.FirstThrowTimingMultiplier
            : settings.ComboTimingMultiplier;

        timingMultiplier *= Mathf.Max(0.1f, variant.TimingMultiplier);

        bool isAccented = settings.AccentEvery > 0 && (throwNumber + 1) % settings.AccentEvery == 0;
        float accentMultiplier = isAccented ? settings.AccentImpactMultiplier : 1f;

        return new WoodcutterDepositThrowPlan(
            anticipationDuration: ReadRange(settings.AnticipationDurationRange) * timingMultiplier,
            releaseDuration: ReadRange(settings.ReleaseDurationRange) * timingMultiplier,
            flightDuration: ReadRange(settings.FlightDurationRange),
            recoveryDuration: ReadRange(settings.RecoveryDurationRange) * timingMultiplier,
            postThrowDelay: Mathf.Max(0f, settings.DelayBetweenLogs),
            arcHeight: ReadRange(settings.ArcHeightRange) * variant.ArcHeightMultiplier * (isAccented ? 1.05f : 1f),
            sideOffset: ReadRange(settings.SideOffsetRange) * variant.SideOffsetMultiplier,
            spinDegrees: ReadRange(settings.SpinDegreesRange) * variant.SpinMultiplier,
            pitchSpin: ReadRange(settings.PitchJitterRange),
            rollSpin: ReadRange(settings.RollJitterRange),
            impactStrength: ReadRange(settings.ImpactStrengthRange) * accentMultiplier,
            apexDurationRatio: ReadRange(settings.ApexDurationRatioRange),
            landingOffset: new Vector3(
                ReadRange(settings.LandingSpreadRange),
                0f,
                ReadRange(settings.LandingSpreadRange)),
            anticipationPose: new WoodcutterDepositPose(
                Vector3.Scale(settings.BodyAnticipationOffset, variant.BodyOffsetScale),
                settings.BodyAnticipationEuler + variant.BodyAnticipationEulerOffset,
                settings.HeldLogLocalPosition + variant.HeldAnticipationOffset,
                settings.HeldLogLocalEuler),
            releasePose: new WoodcutterDepositPose(
                Vector3.Scale(settings.BodyReleaseOffset, variant.BodyOffsetScale) * accentMultiplier,
                settings.BodyReleaseEuler + variant.BodyReleaseEulerOffset,
                settings.HeldLogLocalPosition + variant.HeldReleaseOffset,
                settings.HeldLogLocalEuler + variant.HeldEulerOffset));
    }

    private static ThrowVariantSettings SelectVariant(DepositAnimationSettings settings, int throwNumber, bool startWithVariantB)
    {
        bool useVariantB = (throwNumber + (startWithVariantB ? 1 : 0)) % 2 == 1;
        return useVariantB ? settings.ThrowB : settings.ThrowA;
    }

    private static float ReadRange(Vector2 range)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);
        return Random.Range(min, max);
    }
}
