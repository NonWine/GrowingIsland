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
        Container.BindInstance(new TreeHitAnimationSettings(hitAnimationSettings));
        Container.BindInstance(new TreeFinalFallSettings(finalFallSettings));
        Container.BindInstance(new TreeResourcePickupAnimationSettings(resourcePickupSettings));
    }
}
