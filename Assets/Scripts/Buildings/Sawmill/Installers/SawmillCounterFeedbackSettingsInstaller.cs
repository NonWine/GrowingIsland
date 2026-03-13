using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "SawmillCounterFeedbackSettingsInstaller", menuName = "Installers/SawmillCounterFeedbackSettingsInstaller")]
public class SawmillCounterFeedbackSettingsInstaller : ScriptableObjectInstaller<SawmillCounterFeedbackSettingsInstaller>
{
    [FormerlySerializedAs("_settings")]
    [SerializeField] private SawmillCounterFeedbackSettings settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillCounterFeedbackSettings(settings));
    }
}
