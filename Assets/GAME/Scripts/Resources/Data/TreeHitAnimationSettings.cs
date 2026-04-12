using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class TreeHitAnimationSettings
{
    [Min(0.5f)] public float MainBendAngle = 5.4f;
    [Min(0.25f)] public float OvershootAngle = 1.35f;
    [FormerlySerializedAs("HitBendDuration")]
    [Min(0.01f)] public float BaseHitDuration = 0.05f;
    [Min(0.01f)] public float OvershootDuration = 0.035f;
    [Min(0.01f)] public float SettleDuration = 0.05f;
    [Range(0f, 0.25f)] public float AngleVariance = 0.08f;
    [Range(0f, 0.2f)] public float DurationVariance = 0.05f;


    public TreeHitAnimationSettings() { }

    public TreeHitAnimationSettings(TreeHitAnimationSettings template)
    {
        if (template == null)
            return;

        MainBendAngle = template.MainBendAngle;
        OvershootAngle = template.OvershootAngle;
        BaseHitDuration = template.BaseHitDuration;
        OvershootDuration = template.OvershootDuration;
        SettleDuration = template.SettleDuration;
        AngleVariance = template.AngleVariance;
        DurationVariance = template.DurationVariance;
    }
}
