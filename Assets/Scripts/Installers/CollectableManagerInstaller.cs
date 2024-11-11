using UnityEngine;
using Zenject;

public class CollectableManagerInstaller : MonoInstaller
{
    [SerializeField] private CollectableManager _collectableManager;
    
    public override void InstallBindings()
    {
        
        BindResourcesFactory();
    }

    private void BindResourcesFactory()
    {
        Container.BindInstance(_collectableManager);
    }
}