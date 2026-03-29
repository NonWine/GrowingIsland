using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "SawmillImpactFeedbackSettingsInstaller", menuName = "Installers/SawmillImpactFeedbackSettingsInstaller")]
public class SawmillImpactFeedbackSettingsInstaller : ScriptableObjectInstaller<SawmillImpactFeedbackSettingsInstaller>
{
    [FormerlySerializedAs("_settings")]
    [SerializeField] private SawmillImpactFeedbackSettings settings = new();

    public override void InstallBindings()
    {
        Container.BindInstance(new SawmillImpactFeedbackSettings(settings));
    }
}
