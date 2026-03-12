using Zenject;

public class SawmillInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SawmillView>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<SawmillStorage>().FromMethod(ctx =>
        {
            var config = ctx.Container.Resolve<SawmillConfig>();
            return new SawmillStorage(config.GetLevel(config.StartLevelIndex).StorageCapacity);
        }).AsSingle();
        
        Container.Bind<SawmillUpgrader>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillReward>().AsSingle();
        
        Container.Bind<IWoodcutterWorkplace>().To<SawmillFacade>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterSpawner>().AsSingle();
    }
}
