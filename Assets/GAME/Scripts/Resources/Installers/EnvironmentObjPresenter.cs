using System;
using UnityEngine;
using Zenject;

public class EnvironmentObjPresenter : IInitializable , IDisposable
{
    private IEnvironmentResourceDamageService damageService;
    private EnvironmentPropObjectView propView;
    private EnvironmentResourceEvents events;
    private EnvironmentResourceDropSpawner dropSpawner;
    private ResourceWorld resourceWorld;
    private IResetable resetable;
    private IRespawner respawner;
    
    public EnvironmentObjPresenter(IEnvironmentResourceDamageService damageService, EnvironmentPropObjectView propView)
    {
        this.damageService = damageService;
        this.propView = propView;
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
                dropSpawner.Spawn(resourceWorld.TypeWallet, resourceWorld.VisualDrop, propView.transform.position);
                await respawner.Respawn();
                resetable.Reset();

            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}