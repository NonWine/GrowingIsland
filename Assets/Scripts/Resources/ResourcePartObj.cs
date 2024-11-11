using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class ResourcePartObj : PoolAble , IGameTickable, IPlayerEnterTriggable
{
     [field: SerializeField] public eCollectable TypeE { get; private set; }
    [Inject] private GameController _gameController;
    [Inject] private CollectableManager _collectableWallet;
    private bool _isPicked = true;
    
    
    private void Awake()
    {
        _gameController.RegisterInTick(this);
    }

    protected virtual void Rotate()
    {
        transform.Rotate(Vector3.up, 300 * Time.deltaTime);
    }


    public void Tick()
    {
        if(!gameObject.activeSelf)
            return;
        
        Rotate();
    }

    public override void ResetPool()
    {
        _isPicked = false;
        gameObject.SetActive(true);
    }

    private void PickUp()
    {
        if(_isPicked )
            return;
        _isPicked = true;
        _collectableWallet.GetWallet(TypeE).Add(1);
        transform.DOScale(0f, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }


    public void PlayerEnter()
    {
        PickUp();
    }
}