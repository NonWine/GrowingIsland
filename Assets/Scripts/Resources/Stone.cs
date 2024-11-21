public class Stone: EnvironmentResource
{
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        ParticlePool.Instance.PlayMineHitFx(transform.position);
    }
}