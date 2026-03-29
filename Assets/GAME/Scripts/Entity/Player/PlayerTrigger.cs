using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class PlayerTrigger : MonoBehaviour
{
    [FormerlySerializedAs("_layerMask")]
    [SerializeField] private LayerMask layerMask;


    protected virtual void OnTriggerEnter(Collider other)
    {   
        if ((layerMask.value & (1 << other.gameObject.layer)) == 0)
            return;
        
        if (other.TryGetComponent(out IPlayerEnterTriggable playerEnterTriggable))
        {
            playerEnterTriggable.PlayerEnter();
        }
        else if(other.transform.parent.TryGetComponent(out playerEnterTriggable))
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
        else if (other.transform.parent.TryGetComponent(out playerExitTriggable))
        {
            playerExitTriggable.PlayerExit();
        }
        
    }
}
