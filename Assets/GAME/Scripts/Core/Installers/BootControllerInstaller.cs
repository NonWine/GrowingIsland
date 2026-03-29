using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class BootControllerInstaller : MonoInstaller
{
    [FormerlySerializedAs("_gameController")]
    [SerializeField] private GameController gameController;
    [FormerlySerializedAs("_playerPrefab")]
    [SerializeField] private Player playerPrefab;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
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
        Container.BindInterfacesTo<GameController>().FromInstance(gameController).AsSingle();
    }

    private void RegisterPlayer()
    {
        var playerGO = Object.Instantiate(playerPrefab.gameObject);
        var playerInstance = playerGO.GetComponent<Player>();

        PlayerInstaller.Install(Container, playerInstance);
        Container.InjectGameObject(playerGO);
       }

 
    
 
}

