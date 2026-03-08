using UnityEngine;
using UnityEngine.AI;
using Zenject;


public class WoodcutterView : MonoBehaviour
{
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public WoodcutterWorkSettings Settings { get; private set; }
    
}


public class WoodCutterFacade : ITickable
{
    private readonly WoodcutterStateMachine stateMachine;


    private WoodCutterFacade(WoodcutterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public class Factory : PlaceholderFactory<Sawmill, WoodcutterView> { }
    
    
    public void Tick()
    {
        stateMachine?.Tick();
    }
}