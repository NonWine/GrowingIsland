using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject woodcutterPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<IWoodcutterWorkplace, WoodcutterView, WoodCutterFacade.Factory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<WoodcutterInstaller>(woodcutterPrefab)
            .AsSingle(); 
    }
}