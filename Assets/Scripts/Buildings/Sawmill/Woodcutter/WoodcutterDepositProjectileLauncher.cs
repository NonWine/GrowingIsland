using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public sealed class WoodcutterDepositProjectileLauncher : IWoodcutterDepositProjectileLauncher
{
    private readonly WoodcutterView view;
    private readonly WoodcutterWorkSettings workSettings;

    private readonly List<GameObject> activeProjectiles = new();
    private readonly List<Sequence> activeSequences = new();

    public WoodcutterDepositProjectileLauncher(WoodcutterView view, WoodcutterWorkSettings workSettings)
    {
        this.view = view;
        this.workSettings = workSettings;
    }

    public void ResetSession()
    {
        for (int i = 0; i < activeSequences.Count; i++)
            activeSequences[i]?.Kill();

        for (int i = 0; i < activeProjectiles.Count; i++)
        {
            if (activeProjectiles[i] != null)
                Object.Destroy(activeProjectiles[i]);
        }

        activeSequences.Clear();
        activeProjectiles.Clear();
    }

    public void Launch(WoodcutterReleasedLogData releaseData, WoodcutterDepositThrowPlan plan, TweenCallback onImpactResolved)
    {
        PlayReleaseSound();

        DepositAnimationSettings settings = workSettings.DepositAnimation;
        if (settings.LogPrefab == null)
        {
            onImpactResolved?.Invoke();
            return;
        }

        Vector3 endPosition = releaseData.TargetPosition + plan.LandingOffset;

        GameObject projectile = Object.Instantiate(settings.LogPrefab, releaseData.StartPosition, Quaternion.Euler(releaseData.StartEuler));
        DisablePhysics(projectile);
        activeProjectiles.Add(projectile);

        Transform projectileTransform = projectile.transform;
        Vector3 finalScale = projectileTransform.localScale * settings.ProjectileScale;
        projectileTransform.localScale = finalScale * 0.92f;

        Vector3 direction = endPosition - releaseData.StartPosition;
        Vector3 sideAxis = Vector3.Cross(Vector3.up, direction.normalized);
        if (sideAxis.sqrMagnitude <= 0.0001f)
            sideAxis = view.transform.right;

        Vector3 apexPosition = Vector3.Lerp(releaseData.StartPosition, endPosition, 0.5f)
                               + Vector3.up * plan.ArcHeight
                               + sideAxis * plan.SideOffset;

        float apexDuration = plan.FlightDuration * plan.ApexDurationRatio;
        float descendDuration = Mathf.Max(0.04f, plan.FlightDuration - apexDuration);

        Sequence flightSequence = DOTween.Sequence();
        activeSequences.Add(flightSequence);

        flightSequence.Append(projectileTransform.DOMove(apexPosition, apexDuration).SetEase(Ease.OutCubic));
        flightSequence.Append(projectileTransform.DOMove(endPosition, descendDuration).SetEase(Ease.InQuad));

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(projectileTransform.DOScale(finalScale * settings.ReleaseScaleMultiplier, Mathf.Min(0.06f, apexDuration * 0.45f)).SetEase(Ease.OutBack));
        scaleSequence.Append(projectileTransform.DOScale(finalScale * settings.ApexScaleMultiplier, Mathf.Max(0.04f, apexDuration * 0.55f)).SetEase(Ease.OutSine));
        scaleSequence.Append(projectileTransform.DOScale(finalScale, descendDuration).SetEase(Ease.InSine));
        flightSequence.Join(scaleSequence);

        flightSequence.Join(
            projectileTransform.DORotate(
                releaseData.StartEuler + new Vector3(plan.PitchSpin, plan.SpinDegrees, plan.RollSpin),
                plan.FlightDuration,
                RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

        flightSequence.OnComplete(() =>
        {
            activeSequences.Remove(flightSequence);
            activeProjectiles.Remove(projectile);
            Object.Destroy(projectile);
            onImpactResolved?.Invoke();
        });
    }

    private void PlayReleaseSound()
    {
        DepositAudioSettings settings = workSettings.DepositAudio;
        AudioClip clip = PickRandom(settings.ReleaseClips);
        if (clip == null)
            return;

        if (view.AudioSource != null)
        {
            float originalPitch = view.AudioSource.pitch;
            view.AudioSource.pitch = ReadRange(settings.ReleaseSoundPitchRange);
            view.AudioSource.PlayOneShot(clip, settings.ReleaseVolume);
            view.AudioSource.pitch = originalPitch;
            return;
        }

        AudioSource.PlayClipAtPoint(clip, view.transform.position, settings.ReleaseVolume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[Random.Range(0, clips.Length)];
    }

    private static float ReadRange(Vector2 range)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);
        return Random.Range(min, max);
    }

    private static void DisablePhysics(GameObject visual)
    {
        foreach (Collider collider in visual.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in visual.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }
}
