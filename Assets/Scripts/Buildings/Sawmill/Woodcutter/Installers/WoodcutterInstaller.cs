using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class WoodcutterInstaller : MonoInstaller
{
    [SerializeField] private WoodcutterView woodcutterView;

    private IWoodcutterWorkplace _workplace;

    [Inject]
    public void Construct(IWoodcutterWorkplace workplace)
    {
        _workplace = workplace;
    }

    public override void InstallBindings()
    {
        Components();

        Container.Bind<IWoodcutterWorkplace>().FromInstance(_workplace).AsSingle();
        Container.Bind<NPCAnimator>().AsSingle().WithArguments(1);
        Container.Bind<IWoodcutterSensor>().To<WoodcutterSensor>().AsSingle();
        WoodcutterDepositAnimationInstaller.Install(Container);

        StateMachine();
        
        Container.BindInterfacesAndSelfTo<WoodCutterFacade>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<NavMeshAgentInitializer>().FromNew().AsSingle();
    }

    private void Components()
    {
        Container.Bind<WoodcutterView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NavMeshAgent>().FromInstance(woodcutterView.Agent).AsSingle();
        Container.Bind<Animator>().FromInstance(woodcutterView.Animator).AsSingle();
    }
    
    private void StateMachine()
    {
        Container.DeclareSignal<ChangeWoodcutterStateSignal>();
        Container.Bind<IState>().To<WoodcutterDepositState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterWaitingState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterChopState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterCollectState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterIdleState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterMoveToTreeState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterReturnState>().AsSingle();
        Container.Bind<IState>().To<WoodcutterSearchTreeState>().AsSingle();
        Container.BindInterfacesAndSelfTo<WoodcutterStateMachine>().AsSingle().NonLazy();

    }
}
