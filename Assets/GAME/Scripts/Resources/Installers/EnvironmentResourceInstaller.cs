using UnityEngine;
using Zenject;

public abstract class EnvironmentResourceInstaller : MonoInstaller
{
    private EnvironmentPropObjectView propObjectView;
    
    public override void InstallBindings()
    {
        Container.Bind<EnvironmentResourceEvents>().AsSingle();
        ViewDependencies();
        DamageHandler();
        Container.BindInterfacesAndSelfTo<EnvironmentResourceRespawnService>().AsSingle();
        InstallResourceBindings();
        Container.BindInterfacesAndSelfTo<EnvironmentObjPresenter>().AsSingle();
    }

    private void ViewDependencies()
    {
         propObjectView = GetComponent<EnvironmentPropObjectView>();
         Container.Bind<EnvironmentPropObjectView>().FromInstance(propObjectView).AsSingle();
         Container.BindInstance(propObjectView.ResourceWorldAsset);
    }

    private void DamageHandler()
    {
        Container.BindInterfacesAndSelfTo<PropsDamageService>().AsSingle();
    }

    protected virtual void InstallResourceBindings()
    {
    }
    
}