using UnityEngine;

public sealed class SawmillImpactVfxPlayer : ISawmillImpactVfxPlayer
{
    private readonly ISawmillImpactFeedbackView _view;
    private readonly SawmillImpactFeedbackSettings _settings;

    public SawmillImpactVfxPlayer(ISawmillImpactFeedbackView view, SawmillImpactFeedbackSettings settings)
    {
        _view = view;
        _settings = settings;
    }

    public void Play()
    {
        ParticleSystem prefab = _settings.ImpactVfxPrefab;
        if (prefab == null)
            return;

        ParticleSystem instance = UnityEngine.Object.Instantiate(
            prefab,
            _view.DepositPoint.position,
            Quaternion.identity);

        float lifetime = Mathf.Max(1f, instance.main.duration + instance.main.startLifetime.constantMax);
        UnityEngine.Object.Destroy(instance.gameObject, lifetime);
    }
}
