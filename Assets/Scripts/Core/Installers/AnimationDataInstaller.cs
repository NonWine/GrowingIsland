using UnityEngine;
using Zenject;

public class AnimationDataInstaller : MonoInstaller
{
    [SerializeField] private CollectableAnimationData _collectableAnimationData;
    [SerializeField] private ResourceData _resourceData;

    public override void InstallBindings()
    {
        Container.BindInstance(_collectableAnimationData).AsSingle();
        Container.BindInstance(_resourceData).AsSingle();

    }
}