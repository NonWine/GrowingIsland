using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnvironmentObjPresenter : IInitializable , IDisposable
{
    private EnvironmentPropObjectView propView;
    private EnvironmentResourceEvents events;
    private IEnvPropDamageService damageService;
    private IDropSpawner dropSpawner;
    private IRespawner respawner;
    private List<IResetable> resetables;
    
    public EnvironmentObjPresenter(IEnvPropDamageService damageService, 
        EnvironmentPropObjectView propView,
        EnvironmentResourceEvents events,
        IDropSpawner dropSpawner,
        IRespawner respawner,
        List<IResetable> resetables)
    {
        this.damageService = damageService;
        this.propView = propView;
        this.events = events;
        this.dropSpawner = dropSpawner;
        this.respawner = respawner;
        this.resetables = resetables;
    }

    public void Initialize()
    {
        events.OnReceiveWorldDamage += ApplyDamage;
    }

    public void Dispose()
    {
        events.OnReceiveWorldDamage -= ApplyDamage;

    }

    private async void ApplyDamage(float damage, Vector3 sourceWorldPosition)
    {
        try
        {
            var result = damageService.ApplyDamage(damage, sourceWorldPosition);
       
            if (!result.WasApplied)
            {
                return;
            }
            events.RaiseHitApplied(result);
  
            if (result.IsFinalHit)
            {
                events.RaiseFinalHitEvent(result);
                dropSpawner.Spawn(propView.transform.position);
                await respawner.Respawn();
                resetables.ForEach(r => r.Reset());
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}