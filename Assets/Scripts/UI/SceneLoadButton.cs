using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.Serialization;

[RequireComponent(typeof(Button))]
public class SceneLoadButton : MonoBehaviour
{
    [FormerlySerializedAs("_sceneName")]
    [SerializeField] private string sceneName;
    [FormerlySerializedAs("_useIndex")]
    [SerializeField] private bool useIndex;
    [FormerlySerializedAs("_sceneIndex")]
    [SerializeField] private int sceneIndex;

    private ISceneLoader sceneLoader;
    private Button button;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        this.sceneLoader = sceneLoader;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (useIndex)
        {
            sceneLoader.LoadScene(sceneIndex);
        }
        else
        {
            sceneLoader.LoadScene(sceneName);
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}

