using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITreeFinalFallReaction
{
    UniTask Play(Vector3 sourceWorldPosition);
    void ResetToNeutral();
}