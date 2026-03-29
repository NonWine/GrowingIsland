using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class NavMeshAgentInitializer : IInitializable
{
    private NavMeshAgent agent;

    public NavMeshAgentInitializer(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    private void InitializeNavMesh()
    {
        if (agent == null) return;

        agent.enabled = false;

        if (NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            agent.transform.position = hit.position;
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogWarning($"[Woodcutter] Could not find NavMesh near {agent.transform.position}. Check your NavMesh surface.");
        }
        
        agent.enabled = true;
    }

    public void Initialize()
    {
        InitializeNavMesh();
    }
}