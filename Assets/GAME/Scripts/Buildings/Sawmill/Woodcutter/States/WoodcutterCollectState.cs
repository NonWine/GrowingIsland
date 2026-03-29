using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private readonly IWoodcutterSensor sensor;
    private readonly WoodcutterWorkSettings woodcutterWorkSettings;
    private float timer;
    private float nextPickupAttemptTime;
    private float lastSuccessfulPickupTime = float.MinValue;
    private bool hasPickedAny;
    private const float InitialPickupDelay = 0.45f;
    private const float PickupRetryInterval = 0.08f;
    private const float PickupRetryWindow = 1.1f;
    private const float PostPickupGrace = 0.18f;

    public WoodcutterCollectState(IWoodcutterSensor sensor, WoodcutterWorkSettings workSettings)
    {
        this.sensor = sensor;
        woodcutterWorkSettings = workSettings;
    }

    public override void Enter()
    {
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer < InitialPickupDelay)
            return;

        int reservedWood = woodCutterFacade.CarriedWood + woodCutterFacade.PendingWood;
        bool hasCapacity = reservedWood < woodcutterWorkSettings.CarryCapacity;

        if (hasCapacity && timer >= nextPickupAttemptTime)
        {
            if (PickUpWood())
            {
                hasPickedAny = true;
                lastSuccessfulPickupTime = timer;
            }

            nextPickupAttemptTime = timer + PickupRetryInterval;
        }

        if (woodCutterFacade.PendingWood > 0 || (hasPickedAny && timer < lastSuccessfulPickupTime + PostPickupGrace))
            return;

        if (woodcutterWorkSettings.CarryCapacity <= woodCutterFacade.CarriedWood)
        {
            ChangeState<WoodcutterReturnState>();
            return;
        }

        if (!hasPickedAny && timer < InitialPickupDelay + PickupRetryWindow)
            return;

        ChangeState<WoodcutterSearchTreeState>();
    }

    private bool PickUpWood()
    {
        bool hasPickedAny = false;
        foreach (var drop in sensor.GetDropsInRadius(15))
        {
            int payloadAmount = drop.PayloadAmount;
            if (!drop.PickUp(view.PickupAnchor, CollectStrategyType.NPC, 0, () => woodCutterFacade.ConfirmReservedWood(payloadAmount)))
                continue;

            woodCutterFacade.ReserveWood(payloadAmount);
            hasPickedAny = true;
        }

        return hasPickedAny;
    }

    public override void Exit()
    {
        timer = 0f;
        nextPickupAttemptTime = 0f;
        lastSuccessfulPickupTime = float.MinValue;
        hasPickedAny = false;
    }
}
