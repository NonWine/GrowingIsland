using UnityEngine;

public sealed class SawmillImpactAudioPlayer : ISawmillImpactAudioPlayer
{
    private readonly ISawmillImpactFeedbackView _view;

    public SawmillImpactAudioPlayer(ISawmillImpactFeedbackView view)
    {
        _view = view;
    }

    public void Play()
    {
        SawmillImpactFeedbackSettings feedback = _view.ImpactFeedbackSettings;
        AudioClip clip = PickRandom(feedback.ImpactClips);
        if (clip == null)
            return;

        AudioSource audioSource = _view.AudioSource;
        if (audioSource != null)
        {
            float originalPitch = audioSource.pitch;
            audioSource.pitch = UnityEngine.Random.Range(feedback.AudioPitchRange.x, feedback.AudioPitchRange.y);
            audioSource.PlayOneShot(clip, feedback.AudioVolume);
            audioSource.pitch = originalPitch;
            return;
        }

        AudioSource.PlayClipAtPoint(clip, _view.DepositPoint.position, feedback.AudioVolume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
