using Zenject;

public sealed class WoodcutterDepositAnimationInstaller : Installer<WoodcutterDepositAnimationInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<WoodcutterDepositPlanBuilder>().AsSingle();
        Container.Bind<WoodcutterDepositVisualController>().AsSingle();
        Container.Bind<WoodcutterDepositProjectileLauncher>().AsSingle();
        Container.Bind<WoodcutterDepositThrowSequence>().AsSingle();
        Container.Bind<WoodcutterDepositRoutine>().AsSingle();
        Container.Bind(typeof(IWoodcutterDepositSession), typeof(WoodcutterDepositSession))
            .To<WoodcutterDepositSession>()
            .AsSingle();
    }
}
