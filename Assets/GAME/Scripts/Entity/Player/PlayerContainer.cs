using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using UnityEngine.Serialization;

public class PlayerContainer : MonoBehaviour
{
    [field: SerializeField] public Transform FarmDetectionPoint {get; private set;}
    [SerializeField] private Transform body;
    [SerializeField] private Animator player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerAnimatorEvent playerAnimatorEvent;
    [SerializeField] private ThirdPersonController  playerController;
    
    [Inject] private Joystick joystick;
    
    [field:SerializeField]  public Transform CameraRoot { get; private set; }
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
    
    public Transform Body => body;

    public Joystick Joystick => joystick;

    public Animator Animator => player;

    public PlayerStats PlayerStats => playerStats;

    public PlayerAnimatorEvent PlayerAnimatorEvent => playerAnimatorEvent;
}
