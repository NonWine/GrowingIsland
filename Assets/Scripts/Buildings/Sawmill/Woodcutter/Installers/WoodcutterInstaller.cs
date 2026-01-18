using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class WoodcutterInstaller : MonoInstaller
{
    [Header("Unity Components")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    
    [Header("Settings")]
    [SerializeField] private WoodcutterWorkSettings _settings;

    private Sawmill _sawmill;

    [Inject]
    public void Construct(Sawmill sawmill)
    {
        _sawmill = sawmill;
    }

    public override void InstallBindings()
    {
        // У WoodcutterInstaller.cs додайте:
        Container.Bind<Woodcutter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NavMeshAgent>().FromInstance(_agent).AsSingle();
        Container.Bind<Animator>().FromInstance(_animator).AsSingle();
        Container.Bind<WoodcutterWorkSettings>().FromInstance(_settings).AsSingle();
        Container.Bind<Sawmill>().FromInstance(_sawmill).AsSingle();
        Container.Bind<Transform>().FromInstance(transform).AsSingle();

        Container.Bind<NPCAnimator>().AsSingle().WithArguments(1);
        Container.Bind<WoodcutterSensor>().AsSingle();
        Container.Bind<WoodcutterContext>().AsSingle().NonLazy();
        
        Container.Bind<WoodcutterStateMachine>().FromMethod(ctx => 
        {
            var woodContext = ctx.Container.Resolve<WoodcutterContext>();
            return WoodcutterStateMachine.CreateDefault(woodContext);
        }).AsSingle();
    }
}
