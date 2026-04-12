using UnityEngine;
using Zenject;

public class TreeLeavesBurster
{
    [Inject] private EnvironmentPropObjectView view;
    
    public void PlayLeavesBursts(TreeHitAnimationSettings settings)
    {
        // var leavesPoints = view.LeavesPoints;
        // if (leavesPoints.Length == 0)
        // {
        //     return;
        // }
        //
        // if (Random.value > settings.LeavesHitChance)
        // {
        //     return;
        // }
        //
        // var maxBursts = Mathf.Max(settings.MinLeavesBursts, settings.MaxLeavesBursts);
        // var burstsCount = Random.Range(settings.MinLeavesBursts, maxBursts + 1);
        //
        // for (int i = 0; i < burstsCount; i++)
        // {
        //     var point = leavesPoints[Random.Range(0, leavesPoints.Length)];
        //     var offset = Random.insideUnitSphere * settings.LeavesPositionJitter;
        //     ParticlePool.Instance.PlayFallenLeaves(point.position + offset);
        // }
    }
    
    public void PlayFinalHitBursts(TreeFinalFallSettings settings)
    {
        var particlePool = ParticlePool.Instance;
        var trunkOrigin = view.ReactionRoot.position;
        for (int i = 0; i < settings.FinalTrunkFxBursts; i++)
        {
            particlePool.PlayAxeHitFx(trunkOrigin + RandomExtensions.RandomHorizontalOffset(settings.FinalTrunkFxJitter));
        }
    }
}