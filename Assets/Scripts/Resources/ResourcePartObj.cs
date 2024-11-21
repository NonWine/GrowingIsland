using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class ResourcePartObj : PoolAble , IGameTickable
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

    public void PickUp()
    {
        if(_isPicked )
            return;
        _isPicked = true;
        _collectableWallet.GetWallet(TypeE).Add(1);
        transform.DOScale(0f, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).SetDelay(Random.Range(0f,2f));
    }



}