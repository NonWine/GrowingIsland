using Zenject;

public sealed class WoodcutterDepositAnimationInstaller : Installer<WoodcutterDepositAnimationInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WoodcutterDepositPlanBuilder>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterDepositVisualController>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterDepositProjectileLauncher>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterDepositThrowSequence>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterDepositRoutine>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterDepositSession>().AsSingle();
    }
}
