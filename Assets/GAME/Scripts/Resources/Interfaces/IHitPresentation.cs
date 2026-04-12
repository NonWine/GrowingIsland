using Cysharp.Threading.Tasks;

public interface IHitPresentation
{
    UniTask PlayFinalHitAsync(EnvironmentResourceHitResult hitResult);
    UniTask PlayHitAsync(EnvironmentResourceHitResult hitResult);
}

