using DG.Tweening;
using UnityEngine;
using Zenject;

public abstract class TreeReactionBase : IInitializable
{
    protected readonly EnvironmentPropObjectView View;
    [Inject] protected readonly TreeLeavesBurster  leavesBurster;

    protected TreeReactionBase(EnvironmentPropObjectView view)
    {
        View = view;
    }

    protected static void KillSequence(ref Sequence sequence)
    {
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill(false);
        }
    }

    protected static void CapturePoseOnce(
        Transform target,
        ref bool isCaptured,
        ref Vector3 localPosition,
        ref Quaternion localRotation)
    {
        if (isCaptured)
        {
            return;
        }

        localPosition = target.localPosition;
        localRotation = target.localRotation;
        isCaptured = true;
    }

    protected static void ResetPose(Transform target, Quaternion localRotation, Vector3? localPosition = null)
    {
        target.localRotation = localRotation;

        if (localPosition.HasValue)
        {
            target.localPosition = localPosition.Value;
        }
    }

    protected static void ResetCapturedPose(
        bool isCaptured,
        Transform target,
        Quaternion localRotation,
        Vector3? localPosition = null)
    {
        if (!isCaptured)
        {
            return;
        }

        ResetPose(target, localRotation, localPosition);
    }

    protected virtual void CacheBasePose()
    {
    }

    public virtual void ResetToNeutral()
    {
    }

    protected Vector3 GetAwayDirectionLocal(Transform pivot, Vector3 sourceWorldPosition)
    {
        var awayWorld = pivot.position - sourceWorldPosition;
        awayWorld.y = 0f;

        if (awayWorld.sqrMagnitude < 0.0001f)
        {
            awayWorld = View.transform.forward;
            awayWorld.y = 0f;
        }

        awayWorld.Normalize();

        var parent = pivot.parent;
        if (parent == null)
        {
            return awayWorld;
        }

        var awayLocal = parent.InverseTransformDirection(awayWorld);
        awayLocal.y = 0f;

        if (awayLocal.sqrMagnitude < 0.0001f)
        {
            awayLocal = Vector3.forward;
        }

        return awayLocal.normalized;
    }

    public abstract void Initialize();
}
