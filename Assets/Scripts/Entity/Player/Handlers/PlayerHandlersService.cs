using Zenject;

public class PlayerHandlersService
{
    private PlayerDefaultRadiusDamageHandler _defaultRadiusDamageHandler;
    private PlayerResourceDetector _playerResourceDetector;
    private OverlapSphereHandler _overlapSphereHandler;
    public readonly PlayerAttackHandler playerAttackHandler;
    public readonly TargetDetector targetDetector;
    public PlayerDefaultRadiusDamageHandler DefaultRadiusDamageHandler => _defaultRadiusDamageHandler;
    public PlayerResourceDetector PlayerResourceDetector => _playerResourceDetector;
    
    public PlayerHandlersService(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler)
    {
        _overlapSphereHandler = overlapSphereHandler;
        _playerResourceDetector = new PlayerResourceDetector(playerContainer,_overlapSphereHandler);
        _defaultRadiusDamageHandler = new PlayerDefaultRadiusDamageHandler(playerContainer,_overlapSphereHandler);
        playerAttackHandler = new PlayerAttackHandler(playerContainer.PlayerStats.Damage);
        targetDetector = new TargetDetector(playerContainer.transform, playerContainer.PlayerStats.RadiusDetection, playerContainer.enemyMask);
    }
    
}