using Zenject;

public class SawmillInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SawmillView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ISawmillView>().To<SawmillView>().FromResolve().AsSingle();
        Container.Bind<ISawmillCounterFeedbackView>().To<SawmillView>().FromResolve().AsSingle();
        Container.Bind<ISawmillImpactFeedbackView>().To<SawmillView>().FromResolve().AsSingle();
        Container.Bind<ISawmillPileVisualTarget>().To<SawmillView>().FromResolve().AsSingle();

        Container.BindInterfacesAndSelfTo<SawmillStorage>().FromMethod(ctx =>
        {
            var config = ctx.Container.Resolve<SawmillConfig>();
            return new SawmillStorage(config.GetLevel(config.StartLevelIndex).StorageCapacity);
        }).AsSingle();
        
        Container.Bind<SawmillUpgrader>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillReward>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillPresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillCounterAnimator>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillImpactTransformAnimator>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillImpactAudioPlayer>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillImpactVfxPlayer>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillImpactFeedbackModule>().AsSingle();
        Container.Bind<SawmillPileRuntime>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillPileLayoutCalculator>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillPileStageFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillPileAnimator>().AsSingle();
        Container.BindInterfacesAndSelfTo<SawmillPileVisualizer>().AsSingle();
        Container.Bind<IWoodcutterWorkplace>().To<SawmillWorkplaceFacade>().AsSingle();
        Container.Bind<SawmillWorkplaceFacade>().FromResolve().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterSpawner>().AsSingle();
    }
}
