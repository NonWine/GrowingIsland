using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class ResourcePartObj : PoolAble , IGameTickable
{
     [field: SerializeField] public eCollectable TypeE { get; private set; }
    [Inject] private IGameСontroller _gameController;
    [Inject] private CollectableManager _collectableWallet;
    private bool _isPicked = true;
    private bool _canPickUp;
    public bool IsPicked => _isPicked;
    
    
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

        Rotate();
    }

    public override void ResetPool()
    {
        gameObject.SetActive(true);
    }

    public void PickUp()
    {
  
        _isPicked = true;
        _collectableWallet.GetWallet(TypeE).Add(1);
      Debug.Log("Pick this ");
      DestroyAnim();
    }

    public void DestroyAnim()
    {
        transform.DOScale(0f, 0.25f).SetEase(Ease.InBack).OnComplete(() => { gameObject.SetActive(false); });
    }

    public void SetIdle()
    {
        _isPicked = false;
    }

}