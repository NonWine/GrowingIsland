using UnityEngine;

[System.Serializable]
public class TreeHitAnimationSettings
{
    [Min(0.5f)] public float MainBendAngle = 5.4f;
    [Min(0.25f)] public float OvershootAngle = 1.35f;
    [Min(0.01f)] public float HitBendDuration = 0.055f;
    [Min(0.01f)] public float OvershootDuration = 0.035f;
    [Min(0.01f)] public float SettleDuration = 0.05f;
    [Range(1f, 2f)] public float FinalHitMultiplier = 1.25f;
    [Range(0f, 0.25f)] public float AngleVariance = 0.08f;
    [Range(0f, 0.2f)] public float DurationVariance = 0.05f;
    [Range(0f, 1f)] public float CrownAngleMultiplier = 0.18f;
    [Min(0f)] public float CrownLag = 0.018f;
    [Range(0f, 1f)] public float LeavesHitChance = 0.65f;
    [Min(1)] public int MinLeavesBursts = 1;
    [Min(1)] public int MaxLeavesBursts = 2;
    [Min(0f)] public float LeavesPositionJitter = 0.22f;
}

[System.Serializable]
public class TreeFinalFallSettings
{
    [Range(1.25f, 1.5f)] public float FinalBendMultiplier = 1.35f;
    [Min(0.03f)] public float MicroHoldDuration = 0.045f;
    [Min(0.22f)] public float FallDuration = 0.3f;
    [Min(0.08f)] public float LandImpactDuration = 0.11f;
    [Range(55f, 90f)] public float FallAngle = 74f;
    [Range(0.5f, 8f)] public float ImpactRotationPunch = 2.2f;
    [Range(0f, 0.18f)] public float ImpactPositionPunch = 0.04f;
    [Range(1f, 2.5f)] public float FinalLeavesMultiplier = 1.7f;
    [Min(1)] public int FinalTrunkFxBursts = 2;
    [Min(0f)] public float FinalTrunkFxJitter = 0.2f;
    [Min(1)] public int FinalLeafBurstsMin = 2;
    [Min(1)] public int FinalLeafBurstsMax = 4;
    [Min(0f)] public float FinalLeafPositionJitter = 0.28f;
    [Min(1)] public int ImpactDustBursts = 2;
    [Min(0f)] public float ImpactDustJitter = 0.22f;
}
