using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SawmillCounterFeedbackSettingsInstaller", menuName = "Installers/SawmillCounterFeedbackSettingsInstaller")]
public class SawmillCounterFeedbackSettingsInstaller : ScriptableObjectInstaller<SawmillCounterFeedbackSettingsInstaller>
{
    [SerializeField] private SawmillCounterFeedbackSettings _settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillCounterFeedbackSettings(_settings));
    }
}
