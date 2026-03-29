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
        playerContainer.transform.position = point.position;
        QualitySettings.vSyncCount = 0; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
