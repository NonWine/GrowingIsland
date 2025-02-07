using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [Inject] private PlayerContainer _playerContainer;

    private void Awake()
    {
        _playerContainer.Agent.Warp(_point.position);
    }
}