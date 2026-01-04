using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface ISceneLoader
{
    event Action OnSceneUnloaded;
    event Action OnSceneLoaded;
    void LoadScene(string sceneName);
    void LoadScene(int sceneIndex);
}

public class SceneLoader : ISceneLoader
{
    public event Action OnSceneUnloaded;
    public event Action OnSceneLoaded;

    private readonly ZenjectSceneLoader _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoader zenjectSceneLoader)
    {
        _zenjectSceneLoader = zenjectSceneLoader;
    }

    public void LoadScene(string sceneName)
    {
        PrepareLoad();
        _zenjectSceneLoader.LoadScene(sceneName, LoadSceneMode.Single, container => {
            OnSceneLoaded?.Invoke();
        });
    }

    public void LoadScene(int sceneIndex)
    {
        PrepareLoad();
        _zenjectSceneLoader.LoadScene(sceneIndex, LoadSceneMode.Single, container => {
            OnSceneLoaded?.Invoke();
        });
    }

    private void PrepareLoad()
    {
        OnSceneUnloaded?.Invoke();
    }
}
