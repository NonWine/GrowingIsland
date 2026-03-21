using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class ResourcePartObj : PoolAble, IGameTickable
{
    [field: SerializeField] public eCollectable TypeE { get; private set; }
    [Inject] private IGameController gameController;
    [Inject] private CollectableManager collectableWallet;
    [Inject] private CollectStrategyRegistry collectStrategyRegistry;

    private bool isPicked;
    private bool canBePicked = true;
    private bool autoRotateEnabled = true;
    private int payloadAmount = 1;
    private bool useStylizedMagnetPickup;
    private bool deferRewardUntilPickupImpact;
    private float pickupDelay;
    private float pickupFlyDuration = 0.24f;
    private float pickupArcHeight = 0.12f;
    private float finalPickupPopScale = 1.1f;
    private float finalPickupPopDuration = 0.08f;

    public bool IsPicked => isPicked;
    public bool CanBePicked => canBePicked;
    public int PayloadAmount => payloadAmount;
    public bool UseStylizedMagnetPickup => useStylizedMagnetPickup;
    public float PickupDelay => pickupDelay;
    public float PickupFlyDuration => pickupFlyDuration;
    public float PickupArcHeight => pickupArcHeight;
    public float FinalPickupPopScale => finalPickupPopScale;
    public float FinalPickupPopDuration => finalPickupPopDuration;

    private void Awake()
    {
        gameController.RegisterInTick(this);
    }

    protected virtual void Rotate()
    {
        transform.Rotate(Vector3.up, 300 * Time.deltaTime);
    }

    public void Tick()
    {
        if (!gameObject.activeInHierarchy || !autoRotateEnabled)
            return;

        Rotate();
    }

    public override void ResetPool()
    {
        transform.DOKill(complete: false);
        gameObject.SetActive(true);
        isPicked = false;
        canBePicked = true;
        autoRotateEnabled = true;
        payloadAmount = 1;
        useStylizedMagnetPickup = false;
        deferRewardUntilPickupImpact = false;
        pickupDelay = 0f;
        pickupFlyDuration = 0.24f;
        pickupArcHeight = 0.12f;
        finalPickupPopScale = 1.1f;
        finalPickupPopDuration = 0.08f;
        transform.localScale = Vector3.one;
    }

    public void SetPickupEnabled(bool value)
    {
        canBePicked = value;
    }

    public void SetPayloadAmount(int value)
    {
        payloadAmount = Mathf.Max(1, value);
    }

    public void SetAutoRotateEnabled(bool value)
    {
        autoRotateEnabled = value;
    }

    public void ConfigureStylizedMagnetPickup(
        float delay,
        float flightDuration,
        float arcHeight,
        float popScale,
        float popDuration,
        bool deferRewardUntilImpact)
    {
        useStylizedMagnetPickup = true;
        pickupDelay = Mathf.Max(0f, delay);
        pickupFlyDuration = Mathf.Max(0.01f, flightDuration);
        pickupArcHeight = Mathf.Max(0f, arcHeight);
        finalPickupPopScale = Mathf.Max(1f, popScale);
        finalPickupPopDuration = Mathf.Max(0.01f, popDuration);
        this.deferRewardUntilPickupImpact = deferRewardUntilImpact;
    }

    public bool PickUp(Transform collector, CollectStrategyType strategy, int amount = -1, Action onCollectImpact = null)
    {
        if (isPicked || !canBePicked)
            return false;

        isPicked = true;
        int resolvedAmount = amount >= 0 ? amount : payloadAmount;

        Action combinedImpact = onCollectImpact;
        if (resolvedAmount > 0)
        {
            if (deferRewardUntilPickupImpact)
                combinedImpact += () => collectableWallet.GetWallet(TypeE).Add(resolvedAmount);
            else
                collectableWallet.GetWallet(TypeE).Add(resolvedAmount);
        }

        collectStrategyRegistry.GetStrategy(strategy).Collect(this, collector, combinedImpact);
        return true;
    }
}
