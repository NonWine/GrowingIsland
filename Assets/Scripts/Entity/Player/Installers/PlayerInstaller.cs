using Zenject;

public class PlayerInstaller : Installer<Player, PlayerInstaller>
{
    private readonly Player _player;

    public PlayerInstaller(Player player)
    {
        _player = player;
    }

    public override void InstallBindings()
    {
        BindCoreDependencies();
        BindHandlers();
        BindDetectors();
        Container.Bind<PlayerController>().AsSingle();
    }

    private void BindCoreDependencies()
    {
        Container.BindInstance(_player.PlayerContainer).AsSingle();
        Container.BindInstance(_player).AsSingle();
        Container.Bind<PlayerStateMachine>().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerAnimator>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerRotating>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMoving>().AsSingle();
    }

    private void BindHandlers()
    {
        Container.BindInterfacesAndSelfTo<PlayerDefaultRadiusDamageHandler>().AsSingle();
        Container.Bind<PlayerAttackHandler>()
            .AsSingle()
            .WithArguments(_player.PlayerContainer.PlayerStats.Damage);

        Container.Bind<TargetDetector>()
            .AsSingle()
            .WithArguments(
                _player.PlayerContainer.transform,
                _player.PlayerContainer.PlayerStats.AggroRadius,
                _player.PlayerContainer.enemyMask);

        Container.Bind<IDetectionHandler<ResourcePartObj>>()
            .To<PlayerResourceDetectionHandler>()
            .AsSingle();

        Container.Bind<IDetectionHandler<EnvironmentResource>>()
            .To<PlayerFarmDetectionHandler>()
            .AsSingle();

        Container.Bind<IDetectionHandler<BaseEnemy>>()
            .To<PlayerEnemyDetectionHandler>()
            .AsSingle();
    }

    private void BindDetectors()
    {
        Container.Bind<IPlayerDetector>().To<PlayerResourceDetector>().AsSingle();
        Container.Bind<IPlayerDetector>().To<PlayerFarmDetector>().AsSingle();
        Container.Bind<IPlayerDetector>().To<PlayerEnemyDetector>().AsSingle();
    }
}
