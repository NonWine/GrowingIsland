using UnityEngine;

public interface ITreeHitReaction
{
    void PlayHit(Vector3 sourceWorldPosition, bool isFinalHit = false);
    void ResetToNeutral();
}

public interface ITreeFinalFallReaction
{
    void Play(Vector3 sourceWorldPosition);
    void ResetToNeutral();
}
