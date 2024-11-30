using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;


    protected virtual void OnTriggerEnter(Collider other)
    {   
        if ((_layerMask.value & (1 << other.gameObject.layer)) == 0)
            return;
        
        if (other.TryGetComponent(out IPlayerEnterTriggable playerEnterTriggable))
        {
            playerEnterTriggable.PlayerEnter();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayerExitTriggable playerExitTriggable))
        {
            playerExitTriggable.PlayerExit();
        }
    }
}