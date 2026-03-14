using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class FactoryInstaller : MonoInstaller
{
    [FormerlySerializedAs("_resourcePartObjs")]
    [SerializeField] private List<ResourcePartObj> resourcePartObjs;

    public override void InstallBindings()
    {

        //test
        Container.BindInstance(new CollectStrategyRegistry()).AsSingle();
        Container.Bind<ITreeHitReactionFactory>().To<TreeHitReactionFactory>().AsSingle();

        BindResourcesFactory();
  
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle()
            .WithArguments(Resources.LoadAll<EnemyConfig>("EnemyConfigs").ToList()).NonLazy();
        
    }

    private void BindResourcesFactory()
    {
        Dictionary<eCollectable, ResourcePartObj> dictionary = new Dictionary<eCollectable, ResourcePartObj>();
        foreach (var resourcePartObj in resourcePartObjs)
        {
            dictionary.Add(resourcePartObj.TypeE, resourcePartObj);
        }

        Container.BindInterfacesAndSelfTo<ResourcePartObjFactory>()
            .FromInstance(new ResourcePartObjFactory(dictionary, Container)).AsSingle();
    }
}
