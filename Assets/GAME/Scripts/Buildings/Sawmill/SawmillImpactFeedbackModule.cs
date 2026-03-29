public sealed class SawmillImpactFeedbackModule
{
    private readonly SawmillImpactTransformAnimator animator;
    private readonly SawmillImpactAudioPlayer audioPlayer;
    private readonly SawmillImpactVfxPlayer vfxPlayer;

    public SawmillImpactFeedbackModule(
        SawmillImpactTransformAnimator animator,
        SawmillImpactAudioPlayer audioPlayer,
        SawmillImpactVfxPlayer vfxPlayer)
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
