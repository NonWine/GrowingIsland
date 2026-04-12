using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnvironmentObjPresenter : IInitializable , IDisposable
{
    private EnvironmentPropObjectView propView;
    private IEnvPropDamageService damageService;
    private IDropSpawner dropSpawner;
    private IRespawner respawner;
    private IHitPresentation hitPresentation;
    
    private List<IResetable> resetables;
    
    public EnvironmentObjPresenter(IEnvPropDamageService damageService, 
        EnvironmentPropObjectView propView,
        IDropSpawner dropSpawner,
        IRespawner respawner,
        IHitPresentation hitPresentation,
        List<IResetable> resetables)
    {
        this.damageService = damageService;
        this.propView = propView;
        this.dropSpawner = dropSpawner;
        this.respawner = respawner;
        this.hitPresentation = hitPresentation;
        this.resetables = resetables;
    }

    public void Initialize()
    {
        propView.OnReceiveWorldDamage += ApplyDamage;
    }

    public void Dispose()
    {
        propView.OnReceiveWorldDamage -= ApplyDamage;

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
            
  
            if (result.IsFinalHit)
            {
                await hitPresentation.PlayFinalHitAsync(result);
                dropSpawner.Spawn(propView.transform.position);
                EnvironmentResourceViewUtility.SetChildrenVisible(propView.transform, false);
                await respawner.Respawn();
                resetables.ForEach(r => r.Reset());
            }
            else
            {
                await hitPresentation.PlayHitAsync(result);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
