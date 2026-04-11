using Zenject;

public class StoneInstaller : EnvironmentResourceInstaller
{
    protected override void InstallResourceBindings()
    {
        Container.Bind<IFinalHitPresentation>().To<NoOpFinalHitPresentation>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScalePunchEnvironmentResourceDamageFeedback>().AsSingle();
    }
}
