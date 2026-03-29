using Zenject;

public abstract class EnvironmentResourceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EnvironmentResourceEvents>().AsSingle();
        ViewDependencies();
        DamageHandler();
        
        Container.BindInterfacesAndSelfTo<EnvironmentResourceRespawnService>().AsSingle();

        InstallResourceBindings();
    }

    private void ViewDependencies()
    {
        EnvironmentPropObjectView propObjectView = GetComponent<EnvironmentPropObjectView>();
        Container.Bind<EnvironmentPropObjectView>().FromInstance(propObjectView).AsSingle();
        Container.Bind<IDamageable>().FromInstance(propObjectView).AsSingle();
        Container.Bind<IWorldHitDamageable>().FromInstance(propObjectView).AsSingle();
        Container.BindInstance(propObjectView.ResourceWorldAsset);
    }

    private void DamageHandler()
    {
        Container.BindInterfacesAndSelfTo<PropsDamageService>().AsSingle();
    }

    protected virtual void InstallResourceBindings()
    {
    }
}
