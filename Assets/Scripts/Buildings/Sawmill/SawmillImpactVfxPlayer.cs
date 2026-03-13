using UnityEngine;

public sealed class SawmillImpactVfxPlayer : ISawmillImpactVfxPlayer
{
    private readonly ISawmillImpactFeedbackView _view;

    public SawmillImpactVfxPlayer(ISawmillImpactFeedbackView view)
    {
        _view = view;
    }

    public void Play()
    {
        ParticleSystem prefab = _view.ImpactFeedbackSettings.ImpactVfxPrefab;
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
