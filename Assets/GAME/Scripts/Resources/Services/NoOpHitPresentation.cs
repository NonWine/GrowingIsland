using Cysharp.Threading.Tasks;

public sealed class NoOpHitPresentation : IHitPresentation
{
    public UniTask PlayFinalHitAsync(EnvironmentResourceHitResult hitResult) => UniTask.CompletedTask;

    public UniTask PlayHitAsync(EnvironmentResourceHitResult hitResult) => UniTask.CompletedTask;
}
