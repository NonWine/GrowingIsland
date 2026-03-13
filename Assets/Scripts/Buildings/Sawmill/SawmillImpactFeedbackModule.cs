public sealed class SawmillImpactFeedbackModule : ISawmillImpactFeedback
{
    private readonly ISawmillImpactAnimator _animator;
    private readonly ISawmillImpactAudioPlayer _audioPlayer;
    private readonly ISawmillImpactVfxPlayer _vfxPlayer;

    public SawmillImpactFeedbackModule(
        ISawmillImpactAnimator animator,
        ISawmillImpactAudioPlayer audioPlayer,
        ISawmillImpactVfxPlayer vfxPlayer)
    {
        _animator = animator;
        _audioPlayer = audioPlayer;
        _vfxPlayer = vfxPlayer;
    }

    public void Play(float impactStrength)
    {
        _animator.Play(impactStrength);
        _vfxPlayer.Play();
        _audioPlayer.Play();
    }
}
