using Zenject;

public class PlayerInstaller : Installer<Player, PlayerInstaller>
{
    private readonly Player player;

    public PlayerInstaller(Player player)
    {
        this.player = player;
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
        Container.BindInstance(player.PlayerContainer).AsSingle();
        Container.BindInstance(player).AsSingle();
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
            .WithArguments(player.PlayerContainer.PlayerStats.Damage, player.PlayerContainer.transform);

        Container.Bind<TargetDetector>()
            .AsSingle()
            .WithArguments(
                player.PlayerContainer.transform,
                player.PlayerContainer.PlayerStats.AggroRadius,
                player.PlayerContainer.enemyMask);

        Container.Bind<IDetectionHandler<ResourcePartObj>>()
            .To<PlayerResourceDetectionHandler>()
            .AsSingle();

        Container.Bind<IDetectionHandler<EnvironmentPropObjectView>>()
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

