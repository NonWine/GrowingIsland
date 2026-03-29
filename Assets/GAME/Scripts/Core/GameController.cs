using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour , IGameController
{
    [FormerlySerializedAs("_tickables")]
    [SerializeField] private List<IGameTickable> tickables = new List<IGameTickable>();
    private readonly List<IGameTickable> tickablesSnapshot = new List<IGameTickable>();

    public void RegisterInTick(IGameTickable tickable)
    {
        
        if (!tickables.Contains(tickable))
        {
            tickables.Add(tickable);
        }
        
    }

    public void UnregisterFromTick(IGameTickable tickable)
    {
        if (tickables.Contains(tickable))
        {
            tickables.Remove(tickable);
        }
    }
    
    private void Update()
    {
        // iterate over a snapshot to avoid "collection modified" when tickables register/unregister during Tick
        tickablesSnapshot.Clear();
        tickablesSnapshot.AddRange(tickables);

        for (int i = 0; i < tickablesSnapshot.Count; i++)
        {
            var gameTickable = tickablesSnapshot[i];
            if (gameTickable != null)
                gameTickable.Tick();
        }

    }
}
