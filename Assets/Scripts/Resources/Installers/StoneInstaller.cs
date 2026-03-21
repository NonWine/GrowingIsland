using Zenject;

public class StoneInstaller : EnvironmentResourceInstaller
{
    protected override void InstallResourceBindings()
    {
        Container.BindInterfacesAndSelfTo<ScalePunchEnvironmentResourceDamageFeedback>().AsSingle();
        Container.BindInterfacesAndSelfTo<StoneEnvironmentResourceDamageResultHandler>().AsSingle();
    }
}
