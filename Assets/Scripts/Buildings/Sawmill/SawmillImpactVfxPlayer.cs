using UnityEngine;

public sealed class SawmillImpactVfxPlayer : ISawmillImpactVfxPlayer
{
    private readonly ISawmillImpactFeedbackView view;
    private readonly SawmillImpactFeedbackSettings settings;

    public SawmillImpactVfxPlayer(ISawmillImpactFeedbackView view, SawmillImpactFeedbackSettings settings)
    {
        this.view = view;
        this.settings = settings;
    }

    public void Play()
    {
        ParticleSystem prefab = settings.ImpactVfxPrefab;
        if (prefab == null)
            return;

        ParticleSystem instance = UnityEngine.Object.Instantiate(
            prefab,
            view.DepositPoint.position,
            Quaternion.identity);

        float lifetime = Mathf.Max(1f, instance.main.duration + instance.main.startLifetime.constantMax);
        UnityEngine.Object.Destroy(instance.gameObject, lifetime);
    }
}
