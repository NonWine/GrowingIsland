using System;
using UnityEngine;

[Serializable]
public class DepositAudioSettings
{
    public AudioClip[] ReleaseClips;
    public float ReleaseVolume = 1f;
    public Vector2 ReleaseSoundPitchRange = new(0.95f, 1.05f);

    public DepositAudioSettings() { }

    public DepositAudioSettings(DepositAudioSettings template)
    {
        ReleaseClips = template.ReleaseClips;
        ReleaseVolume = template.ReleaseVolume;
        ReleaseSoundPitchRange = template.ReleaseSoundPitchRange;
    }
}