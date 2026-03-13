using UnityEngine;

public sealed class SawmillImpactAudioPlayer : ISawmillImpactAudioPlayer
{
    private readonly ISawmillImpactFeedbackView _view;
    private readonly SawmillImpactFeedbackSettings _settings;

    public SawmillImpactAudioPlayer(ISawmillImpactFeedbackView view, SawmillImpactFeedbackSettings settings)
    {
        _view = view;
        _settings = settings;
    }

    public void Play()
    {
        AudioClip clip = PickRandom(_settings.ImpactClips);
        if (clip == null)
            return;

        AudioSource audioSource = _view.AudioSource;
        if (audioSource != null)
        {
            float originalPitch = audioSource.pitch;
            audioSource.pitch = UnityEngine.Random.Range(_settings.AudioPitchRange.x, _settings.AudioPitchRange.y);
            audioSource.PlayOneShot(clip, _settings.AudioVolume);
            audioSource.pitch = originalPitch;
            return;
        }

        AudioSource.PlayClipAtPoint(clip, _view.DepositPoint.position, _settings.AudioVolume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
