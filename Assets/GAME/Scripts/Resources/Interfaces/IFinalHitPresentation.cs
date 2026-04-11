using Cysharp.Threading.Tasks;

public interface IFinalHitPresentation
{
    UniTask PlayAsync(EnvironmentResourceHitResult hitResult);
}
