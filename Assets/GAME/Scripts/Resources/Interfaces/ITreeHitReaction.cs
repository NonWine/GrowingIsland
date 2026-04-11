using UnityEngine;

public interface ITreeHitReaction
{
    void PlayHit(Vector3 sourceWorldPosition);
    void ResetToNeutral();
}