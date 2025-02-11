using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [Inject] private PlayerContainer _playerContainer;

    private void Awake()
    {
        _playerContainer.Agent.Warp(_point.position);
        Application.targetFrameRate = 120; // Или 120, если устройство поддерживает
        QualitySettings.vSyncCount = 0; // Отключает VSync
        //   UnityEngine.Android.Permission.RequestUserPermission("android.permission.HIGH_SAMPLING_RATE");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}