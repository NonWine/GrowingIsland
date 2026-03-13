using System;
using UnityEngine;

[Serializable]
public class SawmillImpactFeedbackSettings
{
    public float ScalePunch = 0.24f;
    public float Duration = 0.18f;
    public float PositionShake = 0.12f;
    public int ShakeVibrato = 12;
    public float RotationPunch = 6f;
    public float PileSettlePunch = 0.08f;
    public float PileRotationPunch = 4f;
    public int MaxPileShakeStages = 2;
    public ParticleSystem ImpactVfxPrefab;
    public AudioClip[] ImpactClips;
    public float AudioVolume = 1f;
    public Vector2 AudioPitchRange = new(0.95f, 1.05f);
}