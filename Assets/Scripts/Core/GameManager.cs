using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private StorageManager _storageManager;
    [SerializeField] private CollectableAnimationData _collectableAnimationData;
    [Inject] private DiContainer _diContainer;
    
    private void Awake()
    {
        _storageManager = new StorageManager();
        _diContainer.BindInstance(_storageManager).AsSingle();
        _diContainer.BindInstance(_collectableAnimationData).AsSingle();

    }
}