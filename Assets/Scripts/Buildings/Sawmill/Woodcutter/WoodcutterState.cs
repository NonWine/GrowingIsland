[System.Serializable]
public abstract class WoodcutterState
{
    protected readonly WoodcutterContext Ctx;
    protected readonly WoodcutterStateMachine StateMachine;

    protected WoodcutterState(WoodcutterContext context, WoodcutterStateMachine stateMachine)
    {
        Ctx = context;
        StateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}