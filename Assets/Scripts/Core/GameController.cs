using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour , IGameController
{
    [SerializeField] private List<IGameTickable> _tickables = new List<IGameTickable>();
    private readonly List<IGameTickable> _tickablesSnapshot = new List<IGameTickable>();

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
        // iterate over a snapshot to avoid "collection modified" when tickables register/unregister during Tick
        _tickablesSnapshot.Clear();
        _tickablesSnapshot.AddRange(_tickables);

        for (int i = 0; i < _tickablesSnapshot.Count; i++)
        {
            var gameTickable = _tickablesSnapshot[i];
            if (gameTickable != null)
                gameTickable.Tick();
        }

    }
}
