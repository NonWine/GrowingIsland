using System;
using Zenject;

[System.Serializable]
public abstract class WoodcutterState : IState
{
    [Inject] protected WoodCutterFacade woodCutterFacade;
    [Inject] protected WoodcutterWorkSettings workSettings;
    [Inject] protected WoodcutterView Ctx;
    [Inject] protected SignalBus signalBus;

    protected void ChangeState<T>() where T : IState
    {
        signalBus.Fire(new ChangeWoodcutterStateSignal { TargetStateType = typeof(T) });
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