using UnityEngine;

public readonly struct WoodcutterDepositPose
{
    public readonly Vector3 HeldLocalPosition;
    public readonly Vector3 HeldLocalEuler;

    public WoodcutterDepositPose(Vector3 heldLocalPosition, Vector3 heldLocalEuler)
    {
        HeldLocalPosition = heldLocalPosition;
        HeldLocalEuler = heldLocalEuler;
    }
}

public readonly struct WoodcutterReleasedLogData
{
    public readonly Vector3 StartPosition;
    public readonly Vector3 StartEuler;
    public readonly Vector3 TargetPosition;

    public WoodcutterReleasedLogData(Vector3 startPosition, Vector3 startEuler, Vector3 targetPosition)
    {
        StartPosition = startPosition;
        StartEuler = startEuler;
        TargetPosition = targetPosition;
    }
}

public readonly struct WoodcutterDepositThrowPlan
{
    public readonly float AnticipationDuration;
    public readonly float ReleaseDuration;
    public readonly float FlightDuration;
    public readonly float RecoveryDuration;
    public readonly float PostThrowDelay;
    public readonly float ArcHeight;
    public readonly float SideOffset;
    public readonly float SpinDegrees;
    public readonly float PitchSpin;
    public readonly float RollSpin;
    public readonly float ImpactStrength;
    public readonly float ApexDurationRatio;
    public readonly Vector3 LandingOffset;
    public readonly WoodcutterDepositPose AnticipationPose;
    public readonly WoodcutterDepositPose ReleasePose;

    public WoodcutterDepositThrowPlan(
        float anticipationDuration,
        float releaseDuration,
        float flightDuration,
        float recoveryDuration,
        float postThrowDelay,
        float arcHeight,
        float sideOffset,
        float spinDegrees,
        float pitchSpin,
        float rollSpin,
        float impactStrength,
        float apexDurationRatio,
        Vector3 landingOffset,
        WoodcutterDepositPose anticipationPose,
        WoodcutterDepositPose releasePose)
    {
        AnticipationDuration = anticipationDuration;
        ReleaseDuration = releaseDuration;
        FlightDuration = flightDuration;
        RecoveryDuration = recoveryDuration;
        PostThrowDelay = postThrowDelay;
        ArcHeight = arcHeight;
        SideOffset = sideOffset;
        SpinDegrees = spinDegrees;
        PitchSpin = pitchSpin;
        RollSpin = rollSpin;
        ImpactStrength = impactStrength;
        ApexDurationRatio = apexDurationRatio;
        LandingOffset = landingOffset;
        AnticipationPose = anticipationPose;
        ReleasePose = releasePose;
    }

    public WoodcutterDepositPose CreateRecoveryPose(DepositAnimationSettings settings)
    {
        return new WoodcutterDepositPose(
            settings.HeldLogLocalPosition,
            settings.HeldLogLocalEuler);
    }
}

public enum WoodcutterDepositRoutineResult
{
    Interrupted = 0,
    ContinueSearching = 1,
    WaitForStorage = 2
}
