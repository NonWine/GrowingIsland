using System;
using UnityEngine;

public class PlayerResourcesTrigger : PlayerTrigger
{
    public event Action<eCollectable> CurrentResourceTrigger;
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.TryGetComponent(out EnvironmentPropObjectView environmentResource))
        {
            CurrentResourceTrigger?.Invoke(environmentResource.ResourceType);
        }

    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
