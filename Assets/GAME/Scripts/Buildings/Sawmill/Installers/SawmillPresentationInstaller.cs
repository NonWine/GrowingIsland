using Zenject;

public sealed class SawmillPresentationInstaller : Installer<SawmillPresentationInstaller>
{
    public override void InstallBindings()
    {
      

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
