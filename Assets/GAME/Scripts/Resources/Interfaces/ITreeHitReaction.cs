using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITreeHitReaction
{
    public UniTask Play(Vector3 sourceWorldPosition);
    void ResetToNeutral();
}