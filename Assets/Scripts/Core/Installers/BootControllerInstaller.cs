using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BootControllerInstaller : MonoInstaller
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Player _playerPrefab;
    private PlayerContainer _playerContainer;

        
    public override void InstallBindings()
    {
        BindHandlers();
        BindGameController();
        RegirsterPlayer();
    }

    private void BindHandlers()
    {
        Container.Bind<OverlapSphereHandler>().FromNew().AsSingle();
        Container.Bind<StorageManager>().FromNew().AsSingle();

    }

    private void BindGameController()
    {
        Container.BindInterfacesTo<GameController>().FromInstance(_gameController).AsSingle();
    }

    private void RegirsterPlayer()
    {
        _playerPrefab = Container.InstantiatePrefabForComponent<Player>(_playerPrefab);
        _playerContainer = _playerPrefab.PlayerContainer;
        Container.BindInstance(_playerContainer).AsSingle().Lazy();
        Container.BindInstance(_playerPrefab).AsSingle();
    }

 
    
 
}

