public class PlayerHandlersService
{
    private PlayerDefaultRadiusDamageHandler _defaultRadiusDamageHandler;
    private PlayerResourceDetector _playerResourceDetector;

    public PlayerDefaultRadiusDamageHandler DefaultRadiusDamageHandler => _defaultRadiusDamageHandler;
    public PlayerResourceDetector PlayerResourceDetector => _playerResourceDetector;
    
    public PlayerHandlersService(PlayerContainer playerContainer)
    {
        _playerResourceDetector = new PlayerResourceDetector(playerContainer);
        _defaultRadiusDamageHandler = new PlayerDefaultRadiusDamageHandler(playerContainer);
    }
}