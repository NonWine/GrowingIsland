using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "TreePresentationSettingsInstaller", menuName = "Installers/TreePresentationSettingsInstaller")]
public class TreePresentationSettingsInstaller : ScriptableObjectInstaller<TreePresentationSettingsInstaller>
{
    [SerializeField] private TreeHitAnimationSettings hitAnimationSettings = new();
    [SerializeField] private TreeFinalFallSettings finalFallSettings = new();
    [SerializeField] private TreeResourcePickupAnimationSettings resourcePickupSettings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(hitAnimationSettings);
        Container.BindInstance(finalFallSettings);
        Container.BindInstance(resourcePickupSettings);
    }
}
