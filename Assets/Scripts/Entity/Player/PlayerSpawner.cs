using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class PlayerSpawner : MonoBehaviour
{
    [FormerlySerializedAs("_point")]
    [SerializeField] private Transform point;
    [Inject] private Player playerContainer;
    
    private void Awake()
    {
        playerContainer.PlayerContainer.Agent.Warp(point.position);
        Application.targetFrameRate = 120; // Или 120, если устройство поддерживает
        QualitySettings.vSyncCount = 0; // Отключает VSync
        //   UnityEngine.Android.Permission.RequestUserPermission("android.permission.HIGH_SAMPLING_RATE");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
