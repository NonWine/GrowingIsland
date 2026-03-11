using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SawmillConfigInstaller", menuName = "Installers/SawmillConfigInstaller")]
public class SawmillConfigInstaller : ScriptableObjectInstaller<SawmillConfigInstaller>
{
    [SerializeField] private SawmillConfig _config;

    public override void InstallBindings()
    {
        SawmillConfig config = new SawmillConfig(_config);
        
        Container.BindInstance(config);
    }
}
