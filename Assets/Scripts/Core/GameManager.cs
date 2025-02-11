using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CollectableAnimationData _collectableAnimationData;
    [Inject] private DiContainer _diContainer;
    
    private void Awake()
    {
       
        _diContainer.BindInstance(_collectableAnimationData).AsSingle();

    }
}