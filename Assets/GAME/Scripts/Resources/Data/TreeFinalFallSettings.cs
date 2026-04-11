using UnityEngine;

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

    public float ImpactDelay => MicroHoldDuration + MicroHoldDuration + FallDuration;
    public TreeFinalFallSettings() { }

    public TreeFinalFallSettings(TreeFinalFallSettings template)
    {
        if (template == null)
            return;

        FinalBendMultiplier = template.FinalBendMultiplier;
        MicroHoldDuration = template.MicroHoldDuration;
        FallDuration = template.FallDuration;
        LandImpactDuration = template.LandImpactDuration;
        FallAngle = template.FallAngle;
        ImpactRotationPunch = template.ImpactRotationPunch;
        ImpactPositionPunch = template.ImpactPositionPunch;
        FinalLeavesMultiplier = template.FinalLeavesMultiplier;
        FinalTrunkFxBursts = template.FinalTrunkFxBursts;
        FinalTrunkFxJitter = template.FinalTrunkFxJitter;
        FinalLeafBurstsMin = template.FinalLeafBurstsMin;
        FinalLeafBurstsMax = template.FinalLeafBurstsMax;
        FinalLeafPositionJitter = template.FinalLeafPositionJitter;
        ImpactDustBursts = template.ImpactDustBursts;
        ImpactDustJitter = template.ImpactDustJitter;
    }
}