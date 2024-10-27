using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _body;
    [SerializeField] private Animator _player;
    [SerializeField] private PlayerStats _playerStats;
    [Inject] private Joystick _joystick;
    
    [field: SerializeField] public PlayerTrigger PlayerTrigger { get; private set; }
    
    private Vector3 _direction;

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

    
    public NavMeshAgent Agent => _navMeshAgent;
 
    public Transform Body => _body;

    public Joystick Joystick => _joystick;

    public Animator Animator => _player;

    public PlayerStats PlayerStats => _playerStats;
}