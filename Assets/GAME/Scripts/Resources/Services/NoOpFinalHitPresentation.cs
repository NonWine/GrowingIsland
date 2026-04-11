using Cysharp.Threading.Tasks;

public sealed class NoOpFinalHitPresentation : IFinalHitPresentation
{
    public UniTask PlayAsync(EnvironmentResourceHitResult hitResult)
    {
        return UniTask.CompletedTask;
    }
}
