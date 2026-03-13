using Zenject;

public sealed class SawmillPresentationInstaller : Installer<SawmillPresentationInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind(
                typeof(SawmillView),
                typeof(ISawmillView),
                typeof(ISawmillCounterFeedbackView),
                typeof(ISawmillImpactFeedbackView),
                typeof(ISawmillPileVisualTarget))
            .To<SawmillView>()
            .FromComponentInHierarchy()
            .AsSingle();

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
    }
}
