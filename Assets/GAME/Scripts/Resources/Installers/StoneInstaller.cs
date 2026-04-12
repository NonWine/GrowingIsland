using Zenject;

public class StoneInstaller : EnvironmentResourceInstaller
{
    protected override void InstallResourceBindings()
    {
        Container.Bind<IHitPresentation>().To<NoOpHitPresentation>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScalePunchEnvironmentResourceDamageFeedback>().AsSingle();
    }
}
