using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class WoodcutterInstaller : MonoInstaller
{
    [SerializeField] private WoodcutterView woodcutterView;

    private Sawmill _sawmill;

    [Inject]
    public void Construct(Sawmill sawmill)
    {
        _sawmill = sawmill;
    }

    public override void InstallBindings()
    {
        // У WoodcutterInstaller.cs додайте:
        Components();

        Container.Bind<Sawmill>().FromInstance(_sawmill).AsSingle();
        Container.Bind<NPCAnimator>().AsSingle().WithArguments(1);
        Container.Bind<WoodcutterSensor>().AsSingle();
        Container.Bind<WoodcutterContext>().AsSingle().NonLazy();
        
        Container.Bind<WoodcutterStateMachine>().FromMethod(ctx => 
        {
            var woodContext = ctx.Container.Resolve<WoodcutterContext>();
            return WoodcutterStateMachine.CreateDefault(woodContext);
        }).AsSingle();
        Container.BindInterfacesAndSelfTo<WoodCutterFacade>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<NavMeshAgentInitializer>().FromNew().AsSingle();
    }

    private void Components()
    {
        Container.Bind<WoodcutterView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NavMeshAgent>().FromInstance(woodcutterView.Agent).AsSingle();
        Container.Bind<Animator>().FromInstance(woodcutterView.Animator).AsSingle();
        Container.Bind<WoodcutterWorkSettings>().FromInstance(woodcutterView.Settings).AsSingle();
    }
}
