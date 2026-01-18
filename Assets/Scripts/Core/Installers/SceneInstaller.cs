using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject woodcutterPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<Sawmill, Woodcutter, Woodcutter.Factory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<WoodcutterInstaller>(woodcutterPrefab)
            .AsSingle(); 
    }
}