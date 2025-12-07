using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CollectableAnimationData _collectableAnimationData;
    [Inject] private DiContainer _diContainer;
    
    public static GameState GameState { get; private set; }
    
    private void Awake()
    {
       
        _diContainer.BindInstance(_collectableAnimationData).AsSingle();
    }
    
    public static void SetState(GameState state) => GameState = state;
}

public enum GameState
{
    HomeVillage,
    Expedition
}