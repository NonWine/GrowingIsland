using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("_collectableAnimationData")]
    [SerializeField] private CollectableAnimationData collectableAnimationData;
    [Inject] private DiContainer diContainer;
    
    public static GameState GameState { get; private set; }
    
    private void Awake()
    {
       
        diContainer.BindInstance(collectableAnimationData).AsSingle();
    }
    
    public static void SetState(GameState state) => GameState = state;
}

public enum GameState
{
    HomeVillage,
    Expedition
}
