using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public sealed class WoodcutterDepositProjectileLauncher : IWoodcutterDepositProjectileLauncher
{
    private readonly WoodcutterView _view;
    private readonly WoodcutterWorkSettings _workSettings;

    private readonly List<GameObject> _activeProjectiles = new();
    private readonly List<Sequence> _activeSequences = new();

    public WoodcutterDepositProjectileLauncher(WoodcutterView view, WoodcutterWorkSettings workSettings)
    {
        _view = view;
        _workSettings = workSettings;
    }

    public void ResetSession()
    {
        for (int i = 0; i < _activeSequences.Count; i++)
            _activeSequences[i]?.Kill();

        for (int i = 0; i < _activeProjectiles.Count; i++)
        {
            if (_activeProjectiles[i] != null)
                Object.Destroy(_activeProjectiles[i]);
        }

        _activeSequences.Clear();
        _activeProjectiles.Clear();
    }

    public void Launch(WoodcutterReleasedLogData releaseData, WoodcutterDepositThrowPlan plan, TweenCallback onImpactResolved)
    {
        PlayReleaseSound();

        DepositAnimationSettings settings = _workSettings.DepositAnimation;
        if (settings.LogPrefab == null)
        {
            onImpactResolved?.Invoke();
            return;
        }

        Vector3 endPosition = releaseData.TargetPosition + plan.LandingOffset;

        GameObject projectile = Object.Instantiate(settings.LogPrefab, releaseData.StartPosition, Quaternion.Euler(releaseData.StartEuler));
        DisablePhysics(projectile);
        _activeProjectiles.Add(projectile);

        Transform projectileTransform = projectile.transform;
        Vector3 finalScale = projectileTransform.localScale * settings.ProjectileScale;
        projectileTransform.localScale = finalScale * 0.92f;

        Vector3 direction = endPosition - releaseData.StartPosition;
        Vector3 sideAxis = Vector3.Cross(Vector3.up, direction.normalized);
        if (sideAxis.sqrMagnitude <= 0.0001f)
            sideAxis = _view.transform.right;

        Vector3 apexPosition = Vector3.Lerp(releaseData.StartPosition, endPosition, 0.5f)
                               + Vector3.up * plan.ArcHeight
                               + sideAxis * plan.SideOffset;

        float apexDuration = plan.FlightDuration * plan.ApexDurationRatio;
        float descendDuration = Mathf.Max(0.04f, plan.FlightDuration - apexDuration);

        Sequence flightSequence = DOTween.Sequence();
        _activeSequences.Add(flightSequence);

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
            _activeSequences.Remove(flightSequence);
            _activeProjectiles.Remove(projectile);
            Object.Destroy(projectile);
            onImpactResolved?.Invoke();
        });
    }

    private void PlayReleaseSound()
    {
        DepositAudioSettings settings = _workSettings.DepositAudio;
        AudioClip clip = PickRandom(settings.ReleaseClips);
        if (clip == null)
            return;

        if (_view.AudioSource != null)
        {
            float originalPitch = _view.AudioSource.pitch;
            _view.AudioSource.pitch = ReadRange(settings.ReleaseSoundPitchRange);
            _view.AudioSource.PlayOneShot(clip, settings.ReleaseVolume);
            _view.AudioSource.pitch = originalPitch;
            return;
        }

        AudioSource.PlayClipAtPoint(clip, _view.transform.position, settings.ReleaseVolume);
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
