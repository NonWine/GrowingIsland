using UnityEngine;
using UnityEngine.AI;


public class WoodcutterView : MonoBehaviour
{
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
}