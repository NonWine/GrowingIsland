using UnityEngine;

[System.Serializable]
public class TreeHitReactionSettings
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
}
