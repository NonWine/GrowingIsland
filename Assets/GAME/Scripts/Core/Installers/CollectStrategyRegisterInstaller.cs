using Zenject;

public class CollectStrategyRegisterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance(new CollectStrategyRegistry()).AsSingle();
    } 
}