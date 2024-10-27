using System;
using UnityEngine;
using Zenject;

public class ResourcePartObj : PoolAble , IGameTickable
{
     [field: SerializeField] public eCollectable TypeE { get; private set; }
    [Inject] private GameController _gameController;

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
        
    }
}