using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] private List<ResourcePartObj> _resourcePartObjs;

    public override void InstallBindings()
    {

        //test
        Container.BindInstance(new CollectStrategyRegistry()).AsSingle();

        BindResourcesFactory();
  
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle()
            .WithArguments( Resources.LoadAll<EnemyConfig>("EnemyConfigs").ToList()).NonLazy();
        
    }

    private void BindResourcesFactory()
    {
        Dictionary<eCollectable, ResourcePartObj> dictionary = new Dictionary<eCollectable, ResourcePartObj>();
        foreach (var resourcePartObj in _resourcePartObjs)
        {
            dictionary.Add(resourcePartObj.TypeE, resourcePartObj);
        }

        Container.BindInterfacesAndSelfTo<ResourcePartObjFactory>()
            .FromInstance(new ResourcePartObjFactory(dictionary, Container)).AsSingle();
    }
}