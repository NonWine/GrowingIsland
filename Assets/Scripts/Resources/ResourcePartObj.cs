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
    private bool isPicked = false;

    public bool IsPicked => isPicked;

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
        Rotate();
    }

    public override void ResetPool()
    {
        gameObject.SetActive(true);
        isPicked = false;
        transform.localScale = Vector3.one;
    }

    public bool PickUp(Transform collector, CollectStrategyType strategy, int amount = 1)
    {
        if (isPicked) return false;
        isPicked = true;
        collectableWallet.GetWallet(TypeE).Add(amount);
        collectStrategyRegistry.GetStrategy(strategy).Collect(transform, collector);
        return true;
    }
    

    public void SetIdle()
    {
        isPicked = false;
    }
}