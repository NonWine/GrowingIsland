using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class JoystickInstaller : MonoInstaller
{
    [FormerlySerializedAs("_joystick")]
    [SerializeField] private Joystick joystick;
    
    public override void InstallBindings()
    {
        Container.BindInstance(joystick).AsSingle().NonLazy();
    }
}
