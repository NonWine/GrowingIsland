using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] private List<ResourcePartObj> _resourcePartObjs;
    
    public override void InstallBindings()
    {
        
        BindResourcesFactory();
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