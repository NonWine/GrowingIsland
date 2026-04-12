﻿using DG.Tweening;
 using UnityEngine;

[System.Serializable]
public class TreeFinalFallSettings
{
    [Range(1.25f, 1.5f)] public float FinalBendMultiplier = 1.35f;
    [Min(0.03f)] public float FinalRorationDuration = 0.045f;
    public Ease FinalRorationEase = Ease.InQuad;
    [Min(0.03f)] public float MicroHoldDuration = 0.045f;
    [Min(0.22f)] public float FallDuration = 0.3f;
    [Min(0.08f)] public float LandImpactDuration = 0.11f;
    [Range(55f, 90f)] public float FallAngle = 74f;
    [Range(0.5f, 8f)] public float ImpactRotationPunch = 2.2f;
    [Range(0f, 0.18f)] public float ImpactPositionPunch = 0.04f;
    [Min(1)] public int FinalTrunkFxBursts = 2;
    [Min(0f)] public float FinalTrunkFxJitter = 0.2f;

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
        FinalTrunkFxBursts = template.FinalTrunkFxBursts;
        FinalTrunkFxJitter = template.FinalTrunkFxJitter;

    }
}