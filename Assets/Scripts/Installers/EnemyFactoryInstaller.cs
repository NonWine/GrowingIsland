using UnityEngine;
using Zenject;

public class EnemyFactoryInstaller : MonoInstaller
{
    
    public override void InstallBindings()
    {
        Container.Bind<ObjectPoolEnemy>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyFactory>().FromInstance(new EnemyFactory(Container)).AsSingle();
    }
}