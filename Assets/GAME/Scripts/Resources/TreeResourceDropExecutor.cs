using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeResourceDropExecutor : IDropSpawner
{
    private readonly ResourceWorld resourceWorld;
    private readonly EnvironmentPropObjectView view;
    private readonly TreeResourcePickupAnimationSettings settings;
    private readonly ResourcePartObjFactory resourceFactory;

    public TreeResourceDropExecutor(
        ResourceWorld resourceWorld,
        EnvironmentPropObjectView view,
        TreeResourcePickupAnimationSettings settings,
        ResourcePartObjFactory resourceFactory)
    {
        this.resourceWorld = resourceWorld;
        this.view = view;
        this.settings = settings;
        this.resourceFactory = resourceFactory;
    }

    public void Spawn()
    {
        int totalDropAmount = Mathf.Max(1, resourceWorld.VisualDrop);
        int pieceCount = Mathf.Min(totalDropAmount, settings.GetSpawnCount());
        int basePayload = totalDropAmount / pieceCount;
        int remainder = totalDropAmount % pieceCount;

        for (int i = 0; i < pieceCount; i++)
        {
            int payloadAmount = basePayload + (i < remainder ? 1 : 0);
            SpawnPiece(i, pieceCount, payloadAmount);
        }
    }

    private void SpawnPiece(int index, int pieceCount, int payloadAmount)
    {
        ResourcePartObj piece = CreatePiece();
        if (piece == null)
        {
            return;
        }

        piece.ResetPool();
        piece.transform.SetParent(null, true);
        piece.SetPayloadAmount(payloadAmount);
        piece.SetPickupEnabled(false);
        piece.SetAutoRotateEnabled(false);
        piece.ConfigureStylizedMagnetPickup(
            Random.Range(settings.PickupDelayMin, settings.PickupDelayMax),
            Random.Range(settings.PickupFlightDurationMin, settings.PickupFlightDurationMax),
            Random.Range(settings.PickupArcHeightMin, settings.PickupArcHeightMax),
            settings.FinalPickupPopScale,
            settings.FinalPickupPopDuration,
            deferRewardUntilImpact: true);

        Transform origin = view.ResourceDropOrigin;
        Vector3 startPosition = origin.position + Vector3.up * 0.04f;
        Vector3 landingPosition = BuildLandingPosition(origin.position, index, pieceCount);
        Vector3 launchApex = Vector3.Lerp(startPosition, landingPosition, 0.5f)
                             + Vector3.up * Random.Range(settings.LaunchArcHeightMin, settings.LaunchArcHeightMax);

        Transform pieceTransform = piece.transform;
        pieceTransform.position = startPosition;
        pieceTransform.rotation = Quaternion.Euler(GetSpawnRotation());

        float launchDuration = Random.Range(settings.LaunchDurationMin, settings.LaunchDurationMax);
        float settleDuration = Random.Range(settings.SettleDurationMin, settings.SettleDurationMax);
        float launchUpDuration = launchDuration * 0.58f;
        float launchDownDuration = Mathf.Max(0.04f, launchDuration - launchUpDuration);
        float bounceUpDuration = settleDuration * 0.35f;
        float bounceDownDuration = Mathf.Max(0.03f, settleDuration - bounceUpDuration);

        Vector3 baseScale = pieceTransform.localScale;
        Vector3 squashScale = new(baseScale.x * 1.05f, baseScale.y * settings.SettleSquashScale, baseScale.z * 1.05f);
        Vector3 bouncePosition = landingPosition + Vector3.up * settings.LandBounceHeight;
        Vector3 landingEuler = GetLandingRotation(pieceTransform.eulerAngles);

        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(piece.gameObject);
        sequence.Append(pieceTransform.DOMove(launchApex, launchUpDuration).SetEase(Ease.OutQuad));
        sequence.Append(pieceTransform.DOMove(landingPosition, launchDownDuration).SetEase(Ease.InQuad));
        sequence.Append(pieceTransform.DOMove(bouncePosition, bounceUpDuration).SetEase(Ease.OutQuad));
        sequence.Append(pieceTransform.DOMove(landingPosition, bounceDownDuration).SetEase(Ease.InQuad));

        sequence.Join(pieceTransform.DORotate(landingEuler, launchDuration + settleDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic));

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.AppendInterval(launchDuration);
        scaleSequence.Append(pieceTransform.DOScale(squashScale, bounceUpDuration).SetEase(Ease.OutQuad));
        scaleSequence.Append(pieceTransform.DOScale(baseScale, bounceDownDuration).SetEase(Ease.OutSine));
        sequence.Join(scaleSequence);

        sequence.OnComplete(() =>
        {
            piece.SetPickupEnabled(true);
        });
    }

    private ResourcePartObj CreatePiece()
    {
        return resourceFactory.Create(resourceWorld.TypeWallet, settings.PiecePrefabOverride);
    }

    private Vector3 BuildLandingPosition(Vector3 originPosition, int index, int pieceCount)
    {
        float fanT = pieceCount == 1 ? 0.5f : index / (pieceCount - 1f);
        float fanAngle = Mathf.Lerp(-38f, 38f, fanT) + Random.Range(-12f, 12f);
        Vector3 fanDirection = Quaternion.AngleAxis(fanAngle, Vector3.up) * view.transform.forward;
        Vector3 sideDirection = view.transform.right * Random.Range(-settings.SideOffset * 0.35f, settings.SideOffset * 0.35f);
        float distance = Random.Range(settings.SideOffset * 0.55f, settings.SideOffset);

        Vector3 landingPosition = originPosition + fanDirection * distance + sideDirection;
        landingPosition.y = view.GroundImpactPoint.position.y;
        landingPosition.y += settings.LandingHeightOffset;
        return landingPosition;
    }

    private Vector3 GetSpawnRotation()
    {
        return new Vector3(
            Random.Range(-settings.RandomPitchOffset, settings.RandomPitchOffset),
            Random.Range(-settings.RandomYawOffset, settings.RandomYawOffset),
            Random.Range(-settings.RandomRollOffset, settings.RandomRollOffset));
    }

    private Vector3 GetLandingRotation(Vector3 startEuler)
    {
        return startEuler + new Vector3(
            Random.Range(-settings.RandomPitchOffset, settings.RandomPitchOffset),
            Random.Range(70f, 130f) * Mathf.Sign(Random.value - 0.5f),
            Random.Range(-settings.RandomRollOffset, settings.RandomRollOffset));
    }

    public void Spawn(Vector3 origin)
    {
        Spawn();
    }
}
