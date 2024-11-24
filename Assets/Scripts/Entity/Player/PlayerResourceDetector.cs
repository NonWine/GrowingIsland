using DG.Tweening;
using UnityEngine;

public class PlayerResourceDetector
{
    private PlayerContainer _playerContainer;
    private Collider[] _overlapResults;

    public PlayerResourceDetector(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
        _overlapResults = new Collider[20];
    }

    public void FindResources()
    {
        int count = Physics.OverlapSphereNonAlloc(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            _overlapResults
        );

        for (int i = 0; i < count; i++)
        {
            if (_overlapResults[i].TryGetComponent(out ResourcePartObj resource))
            {
                if (resource.IsPicked == false)
                {
                    resource.PickUp();
                    resource.transform.DOMove(_playerContainer.transform.position, 0.25f).SetEase(Ease.InBack);
                    Debug.Log("PickUp");
                    
                }
            }
        }

    }
}