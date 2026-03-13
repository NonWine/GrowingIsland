using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using UnityEngine.Serialization;

public class PlayerContainer : MonoBehaviour
{
    [field: SerializeField] public Transform FarmDetectionPoint {get; private set;}
    [FormerlySerializedAs("_navMeshAgent")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [FormerlySerializedAs("_body")]
    [SerializeField] private Transform body;
    [FormerlySerializedAs("_player")]
    [SerializeField] private Animator player;
    [FormerlySerializedAs("_playerStats")]
    [SerializeField] private PlayerStats playerStats;
    [FormerlySerializedAs("_playerAnimatorEvent")]
    [SerializeField] private PlayerAnimatorEvent playerAnimatorEvent;
    [Inject] private Joystick joystick;
    
    [field: SerializeField] public PlayerTrigger PlayerTrigger { get; private set; }
    [field: SerializeField] public LayerMask enemyMask;

    public PlayerStateMachine PlayerStateMachine
    {
        get;
        private set;
    }
    
    private Vector3 direction;

    public Vector3 Direction
    {
        get;
        set;
    }
    
    public float Magmitude
    {
        get;
        set;
    }

    
    public NavMeshAgent Agent => navMeshAgent;
 
    public Transform Body => body;

    public Joystick Joystick => joystick;

    public Animator Animator => player;

    public PlayerStats PlayerStats => playerStats;

    public PlayerAnimatorEvent PlayerAnimatorEvent => playerAnimatorEvent;
}
