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
        Container.Bind<Player>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
    }

 
    
 
}

