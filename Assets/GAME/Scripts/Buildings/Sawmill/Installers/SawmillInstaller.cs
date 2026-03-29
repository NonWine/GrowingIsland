using Zenject;

public class SawmillInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SawmillView>()
            .To<SawmillView>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillStorage>().FromMethod(ctx =>
        {
            var config = ctx.Container.Resolve<SawmillConfig>();
            return new SawmillStorage(config.GetLevel(config.StartLevelIndex).StorageCapacity);
        }).AsSingle();
        
        Container.Bind<SawmillUpgrader>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillReward>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillWorkplaceFacade>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterSpawner>().AsSingle();
        SawmillPresentationInstaller.Install(Container);
    }
}
