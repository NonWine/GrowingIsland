using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class CollectableManagerInstaller : MonoInstaller
{
    [FormerlySerializedAs("_collectableManager")]
    [SerializeField] private CollectableManager collectableManager;
    
    public override void InstallBindings()
    {
        
        BindResourcesFactory();
    }

    private void BindResourcesFactory()
    {
        Container.BindInstance(collectableManager);
    }
}
