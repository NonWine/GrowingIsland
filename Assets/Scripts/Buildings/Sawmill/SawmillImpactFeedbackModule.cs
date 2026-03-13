public sealed class SawmillImpactFeedbackModule : ISawmillImpactFeedback
{
    private readonly ISawmillImpactAnimator animator;
    private readonly ISawmillImpactAudioPlayer audioPlayer;
    private readonly ISawmillImpactVfxPlayer vfxPlayer;

    public SawmillImpactFeedbackModule(
        ISawmillImpactAnimator animator,
        ISawmillImpactAudioPlayer audioPlayer,
        ISawmillImpactVfxPlayer vfxPlayer)
    {
        this.animator = animator;
        this.audioPlayer = audioPlayer;
        this.vfxPlayer = vfxPlayer;
    }

    public void Play(float impactStrength)
    {
        animator.Play(impactStrength);
        vfxPlayer.Play();
        audioPlayer.Play();
    }
}
