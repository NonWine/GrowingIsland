using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    private void Awake()
    {
        GameManager.SetState(gameState);
    }
}
