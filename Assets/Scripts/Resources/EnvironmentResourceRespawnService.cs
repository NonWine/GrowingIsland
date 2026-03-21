using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

public sealed class EnvironmentResourceRespawnService : IInitializable, IDisposable
{
    private readonly ResourceWorld resourceWorld;
    private readonly IResetable damageService;
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;

    private CancellationTokenSource respawnCts;

    public EnvironmentResourceRespawnService(
        ResourceWorld resourceWorld,
        IResetable damageService,
        EnvironmentPropObjectView view,
        EnvironmentResourceEvents events)
    {
        this.resourceWorld = resourceWorld;
        this.damageService = damageService;
        this.view = view;
        this.events = events;
    }

    public void Initialize()
    {
        events.PresentationCompleted += OnPresentationCompleted;
    }

    public void Dispose()
    {
        events.PresentationCompleted -= OnPresentationCompleted;
        CancelRespawn();
    }

    private void OnPresentationCompleted()
    {
        if (resourceWorld.RespawnTime <= 0f)
        {
            CompleteRespawn();
            return;
        }

        CancelRespawn();
        respawnCts = new CancellationTokenSource();
        RespawnAsync(respawnCts.Token).Forget();
    }

    private async UniTaskVoid RespawnAsync(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(resourceWorld.RespawnTime), cancellationToken: cancellationToken);
            CompleteRespawn();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void CompleteRespawn()
    {
        CancelRespawn();
        damageService.Reset();
        view.SetResourceVisualsVisible(true);
        events.RaiseRespawnCompleted();
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
