using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SawmillPileVisualSettingsInstaller", menuName = "Installers/SawmillPileVisualSettingsInstaller")]
public class SawmillPileVisualSettingsInstaller : ScriptableObjectInstaller<SawmillPileVisualSettingsInstaller>
{
    [SerializeField] private SawmillPileVisualSettings _settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillPileVisualSettings(_settings));
    }
}
