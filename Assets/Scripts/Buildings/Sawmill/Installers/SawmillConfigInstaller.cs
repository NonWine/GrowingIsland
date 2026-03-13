using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "SawmillConfigInstaller", menuName = "Installers/SawmillConfigInstaller")]
public class SawmillConfigInstaller : ScriptableObjectInstaller<SawmillConfigInstaller>
{
    [FormerlySerializedAs("_config")]
    [SerializeField] private SawmillConfig config;

    public override void InstallBindings()
    {
        SawmillConfig runtimeConfig = new SawmillConfig(config);
        
        Container.BindInstance(runtimeConfig);
    }
}
