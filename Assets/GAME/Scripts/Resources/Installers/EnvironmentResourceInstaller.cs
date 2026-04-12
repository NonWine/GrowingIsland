using UnityEngine;
using Zenject;

public abstract class EnvironmentResourceInstaller : MonoInstaller
{
    private EnvironmentPropObjectView propObjectView;
    
    public override void InstallBindings()
    {
        ViewDependencies();
        Container.BindInterfacesAndSelfTo<EnvironmentResourceRespawnService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PropsDamageService>().AsSingle();
        
        InstallResourceBindings();
        InstallDropSpawner();
        Container.BindInterfacesAndSelfTo<EnvironmentObjPresenter>().AsSingle();
    }

    private void ViewDependencies()
    {
         propObjectView = GetComponent<EnvironmentPropObjectView>();
         Container.Bind<EnvironmentPropObjectView>().FromInstance(propObjectView).AsSingle();
         Container.BindInstance(propObjectView.ResourceWorldAsset);
    }
    

    protected virtual void InstallResourceBindings()
    {
    }

    protected virtual void InstallDropSpawner()
    {
        Container.Bind<IDropSpawner>().To<EnvironmentResourceDropSpawner>().AsSingle();
    }
}
