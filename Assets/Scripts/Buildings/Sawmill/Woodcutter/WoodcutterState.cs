using Zenject;

[System.Serializable]
public abstract class WoodcutterState : IState
{
    [Inject] protected readonly WoodCutterFacade woodCutterFacade;
    [Inject] protected readonly WoodcutterWorkSettings workSettings;
    protected readonly WoodcutterView Ctx;
    protected readonly WoodcutterStateMachine StateMachine;

    protected WoodcutterState(WoodcutterView context, WoodcutterStateMachine stateMachine)
    {
        Ctx = context;
        StateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}

public interface IState
{
    void Enter();
    void Tick();
    void Exit();
}