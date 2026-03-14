using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class Tree2 : EnvironmentResource
{
    [FormerlySerializedAs("_animSettings")]
    [SerializeField] private TreeHitAnimationSettings animSettings = new();
    [SerializeField] private TreeView view;

    [Inject] private ITreeHitReactionFactory hitReactionFactory;

    private ITreeHitReaction hitReaction;

    private void Start()
    {
        hitReaction = hitReactionFactory.Create(view, animSettings);
    }

    private void OnDisable()
    {
        hitReaction?.ResetToNeutral();
    }

    protected override void AnimTrigDamage(Vector3 sourceWorldPosition, bool isFinalHit)
    {
        hitReaction.PlayHit(sourceWorldPosition, isFinalHit);
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        ParticlePool.Instance.PlayAxeHitFx(transform.position);
    }

    public override void GetDamage(float damage, Vector3 sourceWorldPosition)
    {
        base.GetDamage(damage, sourceWorldPosition);
        ParticlePool.Instance.PlayAxeHitFx(transform.position);
    }
}
