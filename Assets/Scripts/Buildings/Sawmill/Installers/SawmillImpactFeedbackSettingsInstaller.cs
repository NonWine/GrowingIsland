using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SawmillImpactFeedbackSettingsInstaller", menuName = "Installers/SawmillImpactFeedbackSettingsInstaller")]
public class SawmillImpactFeedbackSettingsInstaller : ScriptableObjectInstaller<SawmillImpactFeedbackSettingsInstaller>
{
    [SerializeField] private SawmillImpactFeedbackSettings _settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillImpactFeedbackSettings(_settings));
    }
}
