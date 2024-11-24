using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour , IGameСontroller
{
    [SerializeField] private List<IGameTickable> _tickables = new List<IGameTickable>();

    public void RegisterInTick(IGameTickable tickable)
    {
        if (!_tickables.Contains(tickable))
        {
            _tickables.Add(tickable);
        }
        
    }

    public void UnregisterFromTick(IGameTickable tickable)
    {
        if (_tickables.Contains(tickable))
        {
            _tickables.Remove(tickable);
        }
    }
    
    private void Update()
    {
        foreach (var gameTickable in _tickables)
        {
            gameTickable.Tick();
        }

    }
}