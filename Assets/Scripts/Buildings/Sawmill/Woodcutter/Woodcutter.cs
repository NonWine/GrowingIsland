using UnityEngine;
using UnityEngine.AI;
using Zenject;

/// <summary>
/// Основний клас-координатор дроворуба. 
/// </summary>
public class Woodcutter : MonoBehaviour, IGameTickable
{
    private WoodcutterStateMachine _stateMachine;
    private IGameController _gameController;
    private NavMeshAgent _agent;

    [Inject]
    private void Construct(WoodcutterStateMachine stateMachine, IGameController gameController, NavMeshAgent agent)
    {
        _stateMachine = stateMachine;
        _gameController = gameController;
        _agent = agent;
    }

    private void Start()
    {
        InitializeNavMesh();
        _gameController?.RegisterInTick(this);
    }

    private void InitializeNavMesh()
    {
        if (_agent == null) return;

        _agent.enabled = false;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            _agent.Warp(hit.position);
        }
        else
        {
            Debug.LogWarning($"[Woodcutter] Could not find NavMesh near {transform.position}. Check your NavMesh surface.");
        }
        
        _agent.enabled = true;
    }

    private void OnDestroy()
    {
        if (_gameController != null)
        {
            _gameController.UnregisterFromTick(this);
        }
    }

    public void Tick()
    {
        _stateMachine?.Tick();
    }

    public class Factory : PlaceholderFactory<Sawmill, Woodcutter> { }
}
