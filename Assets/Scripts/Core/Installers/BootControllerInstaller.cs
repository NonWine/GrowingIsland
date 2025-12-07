using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BootControllerInstaller : MonoInstaller
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Player _playerPrefab;

    public override void InstallBindings()
    {
        BindHandlers();
        BindGameController();
        RegisterPlayer();
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

    private void RegisterPlayer()
    {
        var playerGO = Object.Instantiate(_playerPrefab.gameObject);
        var playerInstance = playerGO.GetComponent<Player>();

        PlayerInstaller.Install(Container, playerInstance);
        Container.InjectGameObject(playerGO);
       }

 
    
 
}

