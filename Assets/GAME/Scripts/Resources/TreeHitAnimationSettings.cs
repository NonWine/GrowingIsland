using UnityEngine;

[System.Serializable]
public class TreeHitAnimationSettings
{
    [Min(0.5f)] public float MainBendAngle = 5.4f;
    [Min(0.25f)] public float OvershootAngle = 1.35f;
    [Min(0.01f)] public float HitBendDuration = 0.055f;
    [Min(0.01f)] public float OvershootDuration = 0.035f;
    [Min(0.01f)] public float SettleDuration = 0.05f;
    [Range(0f, 0.25f)] public float AngleVariance = 0.08f;
    [Range(0f, 0.2f)] public float DurationVariance = 0.05f;
    [Range(0f, 1f)] public float CrownAngleMultiplier = 0.18f;
    [Min(0f)] public float CrownLag = 0.018f;
    [Range(0f, 1f)] public float LeavesHitChance = 0.65f;
    [Min(1)] public int MinLeavesBursts = 1;
    [Min(1)] public int MaxLeavesBursts = 2;
    [Min(0f)] public float LeavesPositionJitter = 0.22f;

    public TreeHitAnimationSettings() { }

    public TreeHitAnimationSettings(TreeHitAnimationSettings template)
    {
        if (template == null)
            return;

        MainBendAngle = template.MainBendAngle;
        OvershootAngle = template.OvershootAngle;
        HitBendDuration = template.HitBendDuration;
        OvershootDuration = template.OvershootDuration;
        SettleDuration = template.SettleDuration;
        AngleVariance = template.AngleVariance;
        DurationVariance = template.DurationVariance;
        CrownAngleMultiplier = template.CrownAngleMultiplier;
        CrownLag = template.CrownLag;
        LeavesHitChance = template.LeavesHitChance;
        MinLeavesBursts = template.MinLeavesBursts;
        MaxLeavesBursts = template.MaxLeavesBursts;
        LeavesPositionJitter = template.LeavesPositionJitter;
    }
}
