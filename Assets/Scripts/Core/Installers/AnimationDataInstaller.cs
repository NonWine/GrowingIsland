using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class AnimationDataInstaller : MonoInstaller
{
    [FormerlySerializedAs("_collectableAnimationData")]
    [SerializeField] private CollectableAnimationData collectableAnimationData;
    [FormerlySerializedAs("_resourceData")]
    [SerializeField] private ResourceData resourceData;

    public override void InstallBindings()
    {
        Container.BindInstance(collectableAnimationData).AsSingle();
        Container.BindInstance(resourceData).AsSingle();

    }
}
