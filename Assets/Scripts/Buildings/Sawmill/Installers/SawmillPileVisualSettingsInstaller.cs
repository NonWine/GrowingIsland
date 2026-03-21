using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "SawmillPileVisualSettingsInstaller", menuName = "Installers/SawmillPileVisualSettingsInstaller")]
public class SawmillPileVisualSettingsInstaller : ScriptableObjectInstaller<SawmillPileVisualSettingsInstaller>
{
    [FormerlySerializedAs("_settings")]
    [SerializeField] private SawmillPileVisualSettings settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillPileVisualSettings(settings));
    }
}
