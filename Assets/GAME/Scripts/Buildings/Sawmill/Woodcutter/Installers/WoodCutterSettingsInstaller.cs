using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "WoodCutterSettingsInstaller", menuName = "Installers/WoodCutterSettingsInstaller")]
public class WoodCutterSettingsInstaller : ScriptableObjectInstaller<WoodCutterSettingsInstaller>
{
    [SerializeField, HideLabel] private WoodcutterWorkSettings woodcutterWorkSettings;
   
    public override void InstallBindings()
    {
        var runtimeSettings = woodcutterWorkSettings != null
            ? new WoodcutterWorkSettings(woodcutterWorkSettings)
            : new WoodcutterWorkSettings();

        Container.BindInstance(woodcutterWorkSettings);
    }
}