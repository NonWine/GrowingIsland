using UnityEngine;

public sealed class SawmillImpactAudioPlayer
{
    private readonly SawmillView view;
    private readonly SawmillImpactFeedbackSettings settings;

    public SawmillImpactAudioPlayer(SawmillView view, SawmillImpactFeedbackSettings settings)
    {
        this.view = view;
        this.settings = settings;
    }

    public void Play()
    {
        AudioClip clip = PickRandom(settings.ImpactClips);
        if (clip == null)
            return;

        AudioSource audioSource = view.AudioSource;
        if (audioSource != null)
        {
            float originalPitch = audioSource.pitch;
            audioSource.pitch = UnityEngine.Random.Range(settings.AudioPitchRange.x, settings.AudioPitchRange.y);
            audioSource.PlayOneShot(clip, settings.AudioVolume);
            audioSource.pitch = originalPitch;
            return;
        }

        AudioSource.PlayClipAtPoint(clip, view.DepositPoint.position, settings.AudioVolume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
