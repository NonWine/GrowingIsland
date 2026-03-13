using System;
using UnityEngine;

[Serializable]
public class ThrowVariantSettings
{
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
        HeldAnticipationOffset = template.HeldAnticipationOffset;
        HeldReleaseOffset = template.HeldReleaseOffset;
        HeldEulerOffset = template.HeldEulerOffset;
        ArcHeightMultiplier = template.ArcHeightMultiplier;
        SideOffsetMultiplier = template.SideOffsetMultiplier;
        SpinMultiplier = template.SpinMultiplier;
        TimingMultiplier = template.TimingMultiplier;
    }

    public static ThrowVariantSettings Create(
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
