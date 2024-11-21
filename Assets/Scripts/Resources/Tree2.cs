using Zenject;

public class Tree2 : EnvironmentResource
{
    
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        ParticlePool.Instance.PlayAxeHitFx(transform.position);
    }
}