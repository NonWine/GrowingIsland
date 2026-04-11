using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

public sealed class EnvironmentResourceRespawnService : IRespawner
{
    private readonly ResourceWorld resourceWorld;
    private readonly EnvironmentPropObjectView view;
    private CancellationTokenSource respawnCts;

    public EnvironmentResourceRespawnService(ResourceWorld resourceWorld, EnvironmentPropObjectView view)
    {
        this.resourceWorld = resourceWorld;
        this.view = view;
    }


    public void Dispose()
    {
        CancelRespawn();
    }

    public UniTask Respawn()
    {
        CancelRespawn();
        respawnCts = new CancellationTokenSource();
        return RespawnAsync(respawnCts.Token);
    }

    private async UniTask RespawnAsync(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(resourceWorld.RespawnTime), cancellationToken: cancellationToken);
            CancelRespawn();
            EnvironmentResourceViewUtility.SetChildrenVisible(view.transform, true);
            
        }
        catch (OperationCanceledException)
        {
            
        }
    }

    private void CancelRespawn()
    {
        if (respawnCts == null)
        {
            return;
        }
        respawnCts.Cancel();
        respawnCts.Dispose();
        respawnCts = null;
    }
}

public interface IRespawner
{
    UniTask Respawn();
}