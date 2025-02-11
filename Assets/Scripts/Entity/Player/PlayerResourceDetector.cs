using DG.Tweening;
using UnityEngine;

public class PlayerResourceDetector
{
    private PlayerContainer _playerContainer;
    private OverlapSphereHandler _overlapSphereHandler;
    
    public PlayerResourceDetector(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler)
    {
        _playerContainer = playerContainer;
        _overlapSphereHandler = overlapSphereHandler;
    }

    public void FindResources()
    {
        var resources = _overlapSphereHandler.GetFilteredObjects<ResourcePartObj>(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            0,
            resource => !resource.IsPicked
        );

        foreach (var resource in resources)
        {
            resource.PickUp();
            resource.transform.DOMove(_playerContainer.transform.position, 0.25f).SetEase(Ease.InBack);
            Debug.Log("Picked up a resource!");
        }

    }
    
}

public class PlayerFarmDetector
{
    private PlayerContainer _playerContainer;
    private OverlapSphereHandler _overlapSphereHandler;
    private PlayerStateMachine _playerStateMachine;
    private PlayerAnimator _playerAnimator;
    
    public PlayerFarmDetector(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine, PlayerAnimator playerAnimator)
    {
        _playerContainer = playerContainer;
        _overlapSphereHandler = overlapSphereHandler;
        _playerAnimator = playerAnimator;
        _playerStateMachine = playerStateMachine;
    }
    
    public void FindFarmingResources()
    {
        var farmObjects = _overlapSphereHandler.GetFilteredObjects<EnvironmentResource>(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusFarming,
            0,
            resource => resource.isAlive
        );
        
        if(_playerStateMachine.CurrentStateKey == PlayerStateKey.Farming)
            return;
        
        if (farmObjects.Count > 0)
        {
            _playerAnimator.SetFarmingAnim(farmObjects[0].ResourceType);
            _playerStateMachine.ChangeState(PlayerStateKey.Farming);
        }

    }

}

public class PlayerEnemyDetector
{
    private PlayerContainer _playerContainer;
    private OverlapSphereHandler _overlapSphereHandler;
    private PlayerStateMachine _playerStateMachine;
    private PlayerAnimator _playerAnimator;
    
    public PlayerEnemyDetector(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine, PlayerAnimator playerAnimator)
    {
        _playerContainer = playerContainer;
        _overlapSphereHandler = overlapSphereHandler;
        _playerAnimator = playerAnimator;
        _playerStateMachine = playerStateMachine;
    }
    
    public void Find()
    {
        var farmObjects = _overlapSphereHandler.GetFilteredObjects<BaseEnemy>(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            0,
            resource => resource.isAlive,
            true
        );
        
        if(_playerStateMachine.CurrentStateKey == PlayerStateKey.Attack)
            return;
        
        if (farmObjects.Count > 0)
        {
            _playerAnimator.SetFarmingAnim(eCollectable.Wood);
            _playerStateMachine.ChangeState(PlayerStateKey.Attack);
        }

    }

}