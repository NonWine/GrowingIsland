using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private bool _useIndex;
    [SerializeField] private int _sceneIndex;

    private ISceneLoader _sceneLoader;
    private Button _button;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (_useIndex)
        {
            _sceneLoader.LoadScene(_sceneIndex);
        }
        else
        {
            _sceneLoader.LoadScene(_sceneName);
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }
}
